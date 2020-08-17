using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;
#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CompassNavigatorPro
{

	public delegate void OnPOIVisited(CompassProPOI poi);
	public delegate void OnPOIVisible(CompassProPOI poi);
	public delegate void OnPOIHide(CompassProPOI poi);

	[ExecuteInEditMode]
	public partial class CompassPro : MonoBehaviour
	{
		class CompassActiveIcon {
			public CompassProPOI poi;
			public Image image;
			public float lastPosX;
			public string levelName;

			RectTransform _rectTransform;
			public RectTransform rectTransform {
				get { return _rectTransform; }
				set { _rectTransform = value; image = _rectTransform.GetComponent<Image>(); }
			}

			public CompassActiveIcon(CompassProPOI poi) {
				this.poi = poi;
				#if UNITY_5_4_OR_NEWER
				this.levelName = SceneManager.GetActiveScene().name;
				#else
				this.levelName = Application.loadedLevelName;
				#endif
			}
		}

		enum CardinalPoint {
			North = 0,
			West = 1,
			South = 2,
			East = 3
		} 

		#region Internal fields

		public bool isDirty;
		static CompassPro _instance;
		List<CompassActiveIcon> icons;
		float fadeStartTime, prevAlpha;
		CanvasGroup canvasGroup;
		RectTransform compassBackRect;
		Image compassBackImage;
		GameObject compassIconPrefab;
		Text text, textShadow;
		Text title, titleShadow;
		float endTimeOfCurrentTextReveal;
		Material spriteOverlayMat;
		Vector3 lastCamPos, lastCamRot;
		int frameCount;
		StringBuilder titleText;
		AudioSource audioSource;
		Text[] cardinals;
		float[] cardinalsLastPosX;
		int poiVisibleCount;
		bool autoHiding;	// performing autohide fade
		float thisAlpha;

		#endregion

		#region Gameloop lifecycle

#if UNITY_EDITOR
		
		[MenuItem("GameObject/UI/Compass Navigator Pro", false)]
		static void CreateCompassNavigatorPro(MenuCommand menuCommand) {
			// Create a custom game object
			GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/CompassNavigatorPro")) as GameObject;;
			go.name = "CompassNavigatorPro";
			GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
			Selection.activeObject = go;
		}

		[MenuItem("GameObject/UI/Compass Navigator Pro", true)]
		static bool ValidateCreateCompassNavigatorPro(MenuCommand menuCommand) {
			return CompassPro.instance==null;
		}


#endif
		public void OnEnable() {

			icons = new List<CompassActiveIcon>(1000);
			audioSource = GetComponent<AudioSource>();
			spriteOverlayMat = Resources.Load<Material>("Materials/SpriteOverlayUnlit");
			compassIconPrefab = Resources.Load<GameObject>("Prefabs/CompassIcon");
			canvasGroup = transform.GetComponent<CanvasGroup>();
			GameObject compassBack = transform.Find("CompassBack").gameObject;
			compassBackRect = compassBack.GetComponent<RectTransform>();
			compassBackImage = compassBack.GetComponent<Image>();
			text = compassBackRect.transform.Find("Text").GetComponent<Text>();
			textShadow = compassBackRect.transform.Find("TextShadow").GetComponent<Text>();
			text.text = textShadow.text = "";
			title = compassBackRect.transform.Find("Title").GetComponent<Text>();
			titleShadow = compassBackRect.transform.Find("TitleShadow").GetComponent<Text>();
			title.text = titleShadow.text = "";
			canvasGroup.alpha = 0;
			prevAlpha = 0f;
			fadeStartTime = Time.time;
			cardinals = new Text[4];
			if (compassBackRect.transform.Find("CardinalN") == null) {
				Debug.LogError("CompassNavigatorPro gameobject has to be updated. Please delete and add prefab again to this scene.");
				_showCardinalPoints = false;
			} else {
				cardinals[(int)CardinalPoint.North] = compassBackRect.transform.Find("CardinalN").GetComponent<Text>();
				cardinals[(int)CardinalPoint.West] = compassBackRect.transform.Find("CardinalW").GetComponent<Text>();
				cardinals[(int)CardinalPoint.South] = compassBackRect.transform.Find("CardinalS").GetComponent<Text>();
				cardinals[(int)CardinalPoint.East] = compassBackRect.transform.Find("CardinalE").GetComponent<Text>();
			}
			cardinalsLastPosX = new float[4];

			UpdateCompassBarAppearance();
			UpdateCompassBarAlpha();

			if (dontDestroyOnLoad && Application.isPlaying) {
				if (FindObjectsOfType(GetType()).Length > 1) {
					Destroy(gameObject);
					return;
				}
				DontDestroyOnLoad(this);
			}
		}

		public void LateUpdate() {
			UpdateCompassBarAlpha();
			UpdateCompassBarContents();
		}

		#endregion

		#region Internal stuff

		/// <summary>
		/// Update bar icons
		/// </summary>
		void UpdateCompassBarContents() {

			if (!Application.isPlaying || Camera.main == null) return;

			// If camera has not moved, then don't refresh compass bar so often - just once each second in case one POI is moving
			if (lastCamPos == Camera.main.transform.position && lastCamRot == Camera.main.transform.eulerAngles) {
				if (++frameCount<60) return;
				frameCount = 0;
			}
			lastCamPos = Camera.main.transform.position;
			lastCamRot = Camera.main.transform.eulerAngles;

			float visibleDistanceSQR = _visibleDistance * _visibleDistance;
			float visitedDistanceSQR = _visitedDistance * _visitedDistance;
			float nearDistanceSQR = _nearDistance * _nearDistance;
			float barMax = _width * 0.5f - _endCapsWidth / Camera.main.pixelWidth;
			const float visibleDistanceFallOffSQR = 25f * 25f;
			if (titleText==null) titleText = new StringBuilder(); else titleText.Length = 0;

			// Cardinal Points
			UpdateCardinalPoints(barMax);

			// Update Icons
			poiVisibleCount = 0;
			for (int p=0;p<icons.Count;p++) {
				bool iconVisible = false;
				CompassActiveIcon activeIcon = icons[p];
				CompassProPOI poi = activeIcon.poi;
				if (poi==null) {
					if (activeIcon.rectTransform!=null && activeIcon.rectTransform.gameObject!=null) DestroyImmediate (activeIcon.rectTransform.gameObject);
					icons.RemoveAt(p);	// POI no longer registered; remove and exit to prevent indexing errors
					continue;
				}

				// Change in visibility?
				float distanceSqr, distancePlanarSQR;
				distanceSqr = (poi.transform.position - Camera.main.transform.position).sqrMagnitude;
				distanceSqr -= poi.radius;
				if (distanceSqr<=0) distanceSqr = 0.01f;
				poi.distanceToCameraSQR = distanceSqr;

				// Distance has changed, proceed with update...
				if (_use3Ddistance) {
					distancePlanarSQR = distanceSqr;
				} else {
					Vector2 v = new Vector2(poi.transform.position.x - Camera.main.transform.position.x, poi.transform.position.z - Camera.main.transform.position.z);
					distancePlanarSQR = v.sqrMagnitude;
				}
				float factor = distancePlanarSQR / nearDistanceSQR;
				if (poi.showPlayModeGizmo) poi.iconAlpha = Mathf.Lerp (0.65f, 0, 5f * factor);
				poi.iconScale = Vector3.one * Mathf.Lerp (_maxIconSize, _minIconSize, factor);

				// Should we make this POI visible in the compass bar?
				if (poi.distanceToCameraSQR < visibleDistanceSQR && poi.isActiveAndEnabled) {
					if (!poi.isVisible) {
						poi.isVisible = true;
						if (OnPOIVisible!=null) OnPOIVisible(poi);
					}
				} else {
					if (poi.isVisible) {
						poi.isVisible = false;
						if (OnPOIHide!=null) OnPOIHide(poi);
					}
				}

				// Is it same scene?
				#if UNITY_5_4_OR_NEWER
				if (poi.isVisible && poi.dontDestroyOnLoad && !activeIcon.levelName.Equals(SceneManager.GetActiveScene().name)) {
					poi.isVisible = false;
				}
				#else
				if (poi.isVisible && poi.dontDestroyOnLoad && !activeIcon.levelName.Equals(Application.loadedLevelName)) {
					poi.isVisible = false;
				}
				#endif

				// Do we visit this POI for the first time?
				if (poi.isVisible && !poi.isVisited && poi.distanceToCameraSQR < visitedDistanceSQR) {
					poi.isVisited = true;
					if (OnPOIVisited!=null) OnPOIVisited(poi);
					if (audioSource!=null) {
						if (poi.visitedAudioClip!=null) {
							audioSource.PlayOneShot(poi.visitedAudioClip);
						} else if (_visitedDefaultAudioClip!=null) {
							audioSource.PlayOneShot(_visitedDefaultAudioClip);
						}
					}
					ShowPOIDiscoveredText(poi);
					if (poi.hideWhenVisited) poi.enabled = false;
				}

				// Check gizmo
				if (!poi.showPlayModeGizmo && poi.spriteRenderer!=null && poi.spriteRenderer.enabled) {
					poi.spriteRenderer.enabled = false;
				} else if (poi.showPlayModeGizmo) {
					if (poi.spriteRenderer==null) {
						poi.spriteRenderer = poi.transform.GetComponent<SpriteRenderer>();
						if (poi.spriteRenderer==null) {
							poi.spriteRenderer = poi.gameObject.AddComponent<SpriteRenderer>();
							poi.spriteRenderer.material = spriteOverlayMat;
							poi.spriteRenderer.enabled = false;
						}
					}
					if (poi.spriteRenderer!=null && !poi.spriteRenderer.enabled) {
						poi.spriteRenderer.enabled = true;
						poi.spriteRenderer.sprite = poi.iconVisited;
					}
					poi.transform.LookAt(Camera.main.transform.position);
					poi.transform.localScale = Vector3.one * _gizmoScale;
					poi.spriteRenderer.color = new Color(1,1,1,poi.iconAlpha);
				}

				// If POI is not visible, then hide and skip
				if (!poi.isVisible) {
					if (activeIcon.rectTransform!=null) {
						activeIcon.rectTransform.gameObject.SetActive(false);
					}
					if (poi.spriteRenderer!=null && poi.spriteRenderer.enabled) poi.spriteRenderer.enabled = false;
					continue;
				}

				// POI is visible, should we create the icon in the compass bar?
				if (activeIcon.rectTransform==null) {
					GameObject iconGO = Instantiate(compassIconPrefab);
					iconGO.hideFlags = HideFlags.DontSave;
					iconGO.transform.SetParent(compassBackRect.transform, false);
					activeIcon.rectTransform = iconGO.GetComponent<RectTransform>();
				}

				// Position the icon on the compass bar
				Vector3 screenPos = Camera.main.WorldToViewportPoint(poi.transform.position);
				float posX = (screenPos.x - 0.5f + activeIcon.lastPosX) * 0.5f;
				activeIcon.lastPosX = posX;

				// Always show the focused icon in the compass bar; if out of bar, maintain it on the edge with normal scale
				if (poi.clampPosition) {
					if (screenPos.z<0) {
						posX = barMax * -Mathf.Sign (screenPos.x - 0.5f);
						if (poi.iconScale.x>1f) poi.iconScale = Vector3.one;
					} else {
						posX = Mathf.Clamp (posX, -barMax, barMax);
						if (poi.iconScale.x>1f) poi.iconScale = Vector3.one;
					}
					screenPos.z = 0;
				}
				float absPosX = Mathf.Abs (posX);

				// Set icon position
				if ( absPosX>barMax || screenPos.z<0) {
					// Icon outside of bar
					if (activeIcon.rectTransform.gameObject.activeSelf) {
						activeIcon.rectTransform.gameObject.SetActive(false);
					}
				} else {
					// Unhide icon
					if (!activeIcon.rectTransform.gameObject.activeSelf) {
						activeIcon.rectTransform.gameObject.SetActive(true);
					}
					activeIcon.rectTransform.anchorMin = activeIcon.rectTransform.anchorMax = new Vector2(0.5f + posX / _width, 0.5f);
					iconVisible = true;
				}

				// Assign proper icon
				if (iconVisible) {
					poiVisibleCount++;
					if (activeIcon.poi.isVisited) {
						if (activeIcon.image != poi.iconVisited) {
							activeIcon.image.sprite = poi.iconVisited;
						}
					} else if (activeIcon.image != poi.iconNonVisited) {
						activeIcon.image.sprite = poi.iconNonVisited;
					}

					// Scale icon
					if (activeIcon.poi.iconScale!=activeIcon.rectTransform.localScale) {
						activeIcon.rectTransform.localScale = activeIcon.poi.iconScale;
					}

					// Set icon's alpha
					if (visibleDistanceFallOffSQR>0) {
						Color spriteColor = activeIcon.image.color;
						float t = (visibleDistanceSQR - poi.distanceToCameraSQR) / visibleDistanceFallOffSQR;
						spriteColor.a = Mathf.Lerp(0, 1, t);
						activeIcon.image.color = spriteColor;
					}

					// Get title if POI is centered
					if (absPosX<0.01f) {
						titleText.Length = 0;
						if (poi.isVisited) {
							titleText.Append(poi.title);
						}
						// indicate "above" or "below"
						bool addedAlt = false;
						if (poi.transform.position.y > Camera.main.transform.position.y + _sameAltitudeThreshold) {
							if (titleText.Length>0) titleText.Append(" ");
							titleText.Append("(Above");
							addedAlt = true;
						} else if (poi.transform.position.y < Camera.main.transform.position.y - _sameAltitudeThreshold) {
							if (titleText.Length>0) titleText.Append(" ");
							titleText.Append("(Below");
							addedAlt = true;
						}
						if (_showDistance) {
							if (addedAlt) {
								titleText.Append(", ");
							} else {
								if (titleText.Length>0) titleText.Append(" ");
								titleText.Append("(");
							}
							titleText.Append(Mathf.Sqrt(distancePlanarSQR).ToString("F1"));
							titleText.Append(" m)");
						} else if (addedAlt) {
							titleText.Append(")");
						}
					}
				}

			}

			// Update title
			string tt = titleText.ToString();
			if (!title.text.Equals(tt)) {
				title.text = titleShadow.text = tt;
				UpdateTitleAlpha(1.0f);
				UpdateTitleAppearance();
			}
		}

		/// <summary>
		/// If showCardinalPoints is enabled, show N, W, S, E across the compass bar
		/// </summary>
		void UpdateCardinalPoints(float barMax) {

			for (int k=0;k<4;k++) {
				if (!_showCardinalPoints) {
					if (cardinals[k].enabled) {
						cardinals[k].enabled = false;
					}
					continue;
				}
				Vector3 cardinalPointWorldPosition = Camera.main.transform.position;
				switch(k) {
				case (int)CardinalPoint.North: cardinalPointWorldPosition.z += 1f; break;
				case (int)CardinalPoint.West: cardinalPointWorldPosition.x -= 1f; break;
				case (int)CardinalPoint.South: cardinalPointWorldPosition.z -= 1f; break;
				default:
				case (int)CardinalPoint.East: cardinalPointWorldPosition.x += 1f; break;
				}
				Vector3 screenPos = Camera.main.WorldToViewportPoint(cardinalPointWorldPosition);
				float posX = (screenPos.x - 0.5f + cardinalsLastPosX[k]) * 0.5f;
				cardinalsLastPosX[k] = posX;
				float absPosX = Mathf.Abs (posX);

				// Set icon position
				if ( absPosX>barMax || screenPos.z<0) {
					// Icon outside of bar
					if (cardinals[k].enabled) {
						cardinals[k].enabled = false;
					}
				} else {
					// Unhide icon
					if (!cardinals[k].enabled) {
						cardinals[k].enabled = true;
					}
					cardinals[k].rectTransform.anchorMin = cardinals[k].rectTransform.anchorMax = new Vector2(0.5f + posX / _width, 0.5f);
				}
			}

		}



		/// <summary>
		/// Manages compass bar alpha transitions
		/// </summary>
		void UpdateCompassBarAlpha() {

			// Alpha
			if (_alwaysVisibleInEditMode && !Application.isPlaying) {
				thisAlpha = 1.0f;
			} else if (_autoHide) {
				if (!autoHiding) {
					if (poiVisibleCount == 0) {
						if (thisAlpha>0) {
							autoHiding = true;
							fadeStartTime = Time.time;
							prevAlpha = canvasGroup.alpha;
							thisAlpha = 0;
						}
					} else if (poiVisibleCount>0 && thisAlpha == 0) {
						thisAlpha = _alpha;
						autoHiding = true;
						fadeStartTime = Time.time;
						prevAlpha = canvasGroup.alpha;
					}
				}
			} else {
				thisAlpha = _alpha;
			}

			if (thisAlpha!=canvasGroup.alpha) {
				float t = Application.isPlaying ? (Time.time - fadeStartTime) / _fadeDuration: 1.0f;
				canvasGroup.alpha = Mathf.Lerp(prevAlpha, thisAlpha, t);
				if (t>=1) {
					prevAlpha = canvasGroup.alpha;
				}
			} else if (autoHiding) autoHiding = false;
		}

		void UpdateCompassBarAppearance() {

			// Width & Vertical Position
			float anchorMinX = (1 - _width) * 0.5f;
			compassBackRect.anchorMin = new Vector2(anchorMinX, _verticalPosition);
			float anchorMaxX = 1f - anchorMinX;
			compassBackRect.anchorMax = new Vector2(anchorMaxX, _verticalPosition);

			// Style
			Sprite barSprite;
			switch(_style) {
			case COMPASS_STYLE.Rounded: barSprite = Resources.Load<Sprite>("Sprites/Bar2"); break;
			case COMPASS_STYLE.Celtic_White: barSprite = Resources.Load<Sprite>("Sprites/Bar3-White"); break;
			case COMPASS_STYLE.Celtic_Black: barSprite = Resources.Load<Sprite>("Sprites/Bar3-Black"); break;
			default: barSprite = Resources.Load<Sprite>("Sprites/Bar1"); break;
			}
			if (compassBackImage.sprite!=barSprite) {
				compassBackImage.sprite = barSprite;
			}

		}

		void UpdateTextAppearanceEditMode() {
			if (!gameObject.activeInHierarchy) return;
			text.text = textShadow.text =  "SAMPLE TEXT";
			UpdateTextAlpha(1);
			UpdateTextAppearance(text.text);
		}

		void UpdateTextAppearance(string sizeText) {
			// Vertical and horizontal position
//			float posX = -text.cachedTextGenerator.GetPreferredWidth(sizeText, text.GetGenerationSettings(Vector2.zero)) * 0.5f * _textScale;
            text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, _textVerticalPosition, 0);
            text.transform.localScale = Vector3.one * _textScale;
            textShadow.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1f, _textVerticalPosition - 1f, 0);
            textShadow.transform.localScale = Vector3.one * _textScale;
		}

		
		void UpdateTextAlpha(float t) {
			text.color = new Color(text.color.r, text.color.g, text.color.b, t);
			textShadow.color = new Color(0,0,0,t);
		}


		void UpdateTitleAppearanceEditMode() {
			if (!gameObject.activeInHierarchy) return;
			title.text = titleShadow.text =  "SAMPLE TITLE";
			UpdateTitleAlpha(1);
			UpdateTitleAppearance();
		}
		
		void UpdateTitleAppearance() {
			// Vertical and horizontal position
            title.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, _titleVerticalPosition, 0);
            title.transform.localScale = Vector3.one * _titleScale;
            titleShadow.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1f, _titleVerticalPosition - 1f, 0);
            titleShadow.transform.localScale = Vector3.one * _titleScale;
		}

		
		void UpdateTitleAlpha(float t) {
			title.color = new Color(title.color.r, title.color.g, title.color.b, t);
			titleShadow.color = new Color(0,0,0,t);
		}


		void ShowPOIDiscoveredText(CompassProPOI poi) {
			if (poi.visitedText==null) return;
			StartCoroutine(AnimateDiscoverText(poi.visitedText));
		}


		IEnumerator AnimateDiscoverText(string discoverText) {

			int len = discoverText.Length;
			if (len==0 || Camera.main==null) yield break;

			while (Time.time < endTimeOfCurrentTextReveal) {
				yield return new WaitForSeconds(1);
			}

            text.text = textShadow.text = "";

            float now = Time.time;
			endTimeOfCurrentTextReveal = now + _textRevealDuration + _textDuration + _textFadeOutDuration * 0.5f;
			UpdateTextAppearance(discoverText);

            // initial pos of text
            string discoverTextSpread = discoverText.Replace(" ", "A");
            float posX = - text.cachedTextGenerator.GetPreferredWidth(discoverTextSpread, text.GetGenerationSettings(Vector2.zero)) * 0.5f * _textScale;

			float acum = 0;
			for (int k =0;k<len;k++) {
				string ch = discoverText.Substring(k,1);

				// Setup shadow (first, so it goes behind white text)
				GameObject letterShadow = Instantiate(textShadow.gameObject);
				letterShadow.transform.SetParent(text.transform.parent, false);
				Text lts = letterShadow.GetComponent<Text>();
				lts.text = ch;

				// Setup letter
				GameObject letter = Instantiate(text.gameObject);
				letter.transform.SetParent(text.transform.parent, false);
                Text lt = letter.GetComponent<Text>();
				lt.text = ch;

				float letw = 0;
				if (ch.Equals(" ")) {
					letw = lt.cachedTextGenerator.GetPreferredWidth("A", lt.GetGenerationSettings(Vector2.zero)) * _textScale;
				} else {
					letw = lt.cachedTextGenerator.GetPreferredWidth(ch, lt.GetGenerationSettings(Vector2.zero)) * _textScale;
				}

				RectTransform letterRT = letter.GetComponent<RectTransform>();
				letterRT.anchoredPosition3D = new Vector3(posX + acum + letw * 0.5f, letterRT.anchoredPosition3D.y, 0);
				RectTransform shadowRT = letterShadow.GetComponent<RectTransform>();
				shadowRT.anchoredPosition3D = new Vector3(posX + acum + letw * 0.5f + 1f, shadowRT.anchoredPosition3D.y, 0);

				acum += letw;

                // Trigger animator
                LetterAnimator anim = letterShadow.AddComponent<LetterAnimator>();
                anim.text = lt;
                anim.textShadow = lts;
                anim.startTime = now + k * _textRevealLetterDelay;
                anim.revealDuration = _textRevealDuration;
                anim.startFadeTime = now + _textRevealDuration + _textDuration;
                anim.fadeDuration = _textFadeOutDuration;
            }
		}

		#endregion
	
	}



}



