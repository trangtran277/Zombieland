﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompassNavigatorPro
{
	public enum COMPASS_STYLE {
		Angled = 0,
		Rounded = 1,
		Celtic_White = 2,
		Celtic_Black = 3
	}

	public partial class CompassPro : MonoBehaviour
	{
		#region Public properties

		[SerializeField]
		COMPASS_STYLE _style = COMPASS_STYLE.Celtic_White;

		public COMPASS_STYLE style {
			get { return _style; }
			set { if (value!=_style) { _style = value; UpdateCompassBarAppearance(); isDirty = true; } }
		}

		[SerializeField]
		float _visibleDistance = 500f;

		/// <summary>
		/// Gets or sets the maximum distance to a POI so it's visible in the compass bar.
		/// </summary>
		public float visibleDistance  {
			get { return _visibleDistance; }
			set { if (value!=_visibleDistance) { _visibleDistance = value;  isDirty = true; } }
		}

		[SerializeField]
		float _nearDistance = 75f;
		
		/// <summary>
		/// Gets or sets the distance to a POI where the icon will start to grow as player approaches.
		/// </summary>
		public float nearDistance  {
			get { return _nearDistance; }
			set { if (value!=_nearDistance) { _nearDistance = value; isDirty = true; } }
		}

		[SerializeField]
		float _visitedDistance = 5f;

		/// <summary>
		/// Gets or sets the minimum distance required to consider a POI as visited. Once the player gets near this POI below this distance, the POI will be marked as visited.
		/// </summary>
		public float visitedDistance {
			get { return _visitedDistance; }
			set { if (value!=_visitedDistance) { _visitedDistance = value; isDirty = true; } }
		}
		
		[SerializeField]
		float _gizmoScale = 0.25f;
		
		/// <summary>
		/// Gets or sets the gizmo scale during playmode.
		/// </summary>
		public float gizmoScale {
			get { return _gizmoScale; }
			set { if (value!=_gizmoScale) { _gizmoScale = value; isDirty = true; } }
		}

		[SerializeField]
		float _alpha = 1.0f;

		/// <summary>
		/// The alpha (transparency) of the compass bar. Setting this value will make the bar shift smoothly from current alpha to the new value (see fadeDuration).
		/// </summary>
		public float alpha {
			get { return _alpha; }
			set { if (value!=_alpha) { _alpha = value; UpdateCompassBarAlpha(); isDirty = true; } }
		}


		[SerializeField]
		bool _autoHide = false;
		
		/// <summary>
		/// If no POIs are below the visible distance param, hide the compass bar
		/// </summary>
		public bool autoHide {
			get { return _autoHide; }
			set { if (value!=_autoHide) { _autoHide = value; isDirty = true; } }
		}



		[SerializeField]
		float _fadeDuration = 2.0f;

		/// <summary>
		/// Sets the duration for any alpha change.
		/// </summary>
		public float fadeDuration {
			get { return _fadeDuration; }
			set { if (value!=_fadeDuration) { _fadeDuration = value; isDirty = true; } }
		}

		[SerializeField]
		bool _alwaysVisibleInEditMode = true;

		/// <summary>
		/// Set this value to true to make the compass bar always visible in Edit Mode (ignores alpha property while editing).
		/// </summary>
		public bool alwaysVisibleInEditMode {
			get { return _alwaysVisibleInEditMode; }
			set { if (value!=_alwaysVisibleInEditMode) { _alwaysVisibleInEditMode = value; UpdateCompassBarAlpha(); isDirty = true; } }
		}

		[SerializeField]
		float _verticalPosition = 0.97f;

		/// <summary>
		/// Distance in % of the screen from the bottom edge of the screen.
		/// </summary>
		public float verticalPosition {
			get { return _verticalPosition; }
			set { if (value!=_verticalPosition) { _verticalPosition = value; UpdateCompassBarAppearance(); isDirty = true; } }
		}

		[SerializeField]
		float _width = 0.65f;
		
		/// <summary>
		/// Width of the compass bar in % of the screen width.
		/// </summary>
		public float width {
			get { return _width; }
			set { if (value!=_width) { _width = value; UpdateCompassBarAppearance(); isDirty = true; } }
		}

		
		[SerializeField]
		float _endCapsWidth = 54f;
		
		/// <summary>
		/// Width of the end caps for the compass bar.
		/// </summary>
		public float endCapsWidth {
			get { return _endCapsWidth; }
			set { if (value!=_endCapsWidth) { _endCapsWidth = value; UpdateCompassBarContents(); isDirty = true; } }
		}

		
		[SerializeField]
		bool _showCardinalPoints = true;
		
		/// <summary>
		/// Whether cardinal points (N, W, S, E) should be visible in the compass bar
		/// </summary>
		public bool showCardinalPoints {
			get { return _showCardinalPoints; }
			set { if (value!=_showCardinalPoints) { _showCardinalPoints = value; UpdateCompassBarContents(); isDirty = true; } }
		}

		[SerializeField]
		float _maxIconSize = 1.15f;
		
		/// <summary>
		/// Maximum icon size. Icons grow or shrinks in the compass bar depending on distance.
		/// </summary>
		public float maxIconSize {
			get { return _maxIconSize; }
			set { if (value!=_maxIconSize) { _maxIconSize = value; isDirty = true; } }
		}

		[SerializeField]
		float _minIconSize = 0.5f;
		
		/// <summary>
		/// Minimum icon size. Icons grow or shrinks in the compass bar depending on distance.
		/// </summary>
		public float minIconSize {
			get { return _minIconSize; }
			set { if (value!=_minIconSize) { _minIconSize = value; isDirty = true; } }
		}


		[SerializeField]
		float _textVerticalPosition = -30;
		
		/// <summary>
		/// Vertical offset for the text of POIs when visited for first time
		/// </summary>
		public float textVerticalPosition {
			get { return _textVerticalPosition; }
			set { if (value!=_textVerticalPosition) { _textVerticalPosition = value; UpdateTextAppearanceEditMode(); isDirty = true; } }
		}

		[SerializeField]
		float _textScale = 0.2f;
		
		/// <summary>
		/// Scaling applied to text
		/// </summary>
		public float textScale {
			get { return _textScale; }
			set { if (value!=_textScale) { _textScale = value; UpdateTextAppearanceEditMode(); isDirty = true; } }
		}

		[SerializeField]
		float _textRevealDuration = 0.5f;
		
		/// <summary>
		/// Duration of the text reveal
		/// </summary>
		public float textRevealDuration {
			get { return _textRevealDuration; }
			set { if (value!=_textRevealDuration) { _textRevealDuration = value; isDirty = true; } }
		}
				
		[SerializeField]
		float _textRevealLetterDelay = 0.05f;
		
		/// <summary>
		/// Delay in appearance of each letter during a text reveal
		/// </summary>
		public float textRevealLetterDelay {
			get { return _textRevealLetterDelay; }
			set { if (value!=_textRevealLetterDelay) { _textRevealLetterDelay = value; isDirty = true; } }
		}

		[SerializeField]
		float _textDuration = 5f;
		
		/// <summary>
		/// Duration of the text on screen before fading out
		/// </summary>
		public float textDuration {
			get { return _textDuration; }
			set { if (value!=_textDuration) { _textDuration = value; isDirty = true; } }
		}

		[SerializeField]
		float _textFadeOutDuration = 2f;
		
		/// <summary>
		/// Duration of the text fade out
		/// </summary>
		public float textFadeOutDuration {
			get { return _textFadeOutDuration; }
			set { if (value!=_textFadeOutDuration) { _textFadeOutDuration = value; isDirty = true; } }
		}

		
		[SerializeField]
		float _titleVerticalPosition = 18f;
		
		/// <summary>
		/// Vertical offset for the title of the (visited/known) centered POI in the compass bar
		/// </summary>
		public float titleVerticalPosition {
			get { return _titleVerticalPosition; }
			set { if (value!=_titleVerticalPosition) { _titleVerticalPosition = value; UpdateTitleAppearanceEditMode(); isDirty = true; } }
		}

		
		[SerializeField]
		float _titleScale = 0.1f;
		
		/// <summary>
		/// Scaling applied to title
		/// </summary>
		public float titleScale {
			get { return _titleScale; }
			set { if (value!=_titleScale) { _titleScale = value; UpdateTitleAppearanceEditMode(); isDirty = true; } }
		}

		[SerializeField]
		bool _use3Ddistance = false;
		
		/// <summary>
		/// Check whether 3D distance should be computed instead of planar X/Z distance.
		/// </summary>
		public bool use3Ddistance {
			get { return _use3Ddistance; }
			set { if (value!=_use3Ddistance) { _use3Ddistance = value; isDirty = true; } }
		}

		
		[SerializeField]
		float _sameAltitudeThreshold = 3f;
		
		/// <summary>
		/// Minimum difference in altitude from camera to show "above" or "below"
		/// </summary>
		public float sameAltitudeThreshold {
			get { return _sameAltitudeThreshold; }
			set { if (value!=_sameAltitudeThreshold) { _sameAltitudeThreshold = value; isDirty = true; } }
		}

		
		[SerializeField]
		bool _showDistance = false;
		
		/// <summary>
		/// Whether the distance in meters should be shown next to the title
		/// </summary>
		public bool showDistance {
			get { return _showDistance; }
			set { if (value!=_showDistance) { _showDistance = value; isDirty = true; } }
		}

		[SerializeField]
		AudioClip _visitedDefaultAudioClip;

		/// <summary>
		/// Default audio clip to play when a POI is visited the first time. Note that you can specify a different audio clip in the POI script itself.
		/// </summary>
		public AudioClip visitedDefaultAudioClip {
			get { return _visitedDefaultAudioClip; }
			set { if (value!=_visitedDefaultAudioClip) { _visitedDefaultAudioClip = value; isDirty = true; } }
		}

		
		[SerializeField]
		AudioClip _beaconDefaultAudioClip;
		
		/// <summary>
		/// Default audio clip to play when a POI beacon is shown (see manual for more info about POI beacons).
		/// </summary>
		public AudioClip beaconDefaultAudioClip {
			get { return _beaconDefaultAudioClip; }
			set { if (value!=_beaconDefaultAudioClip) { _beaconDefaultAudioClip = value; isDirty = true; } }
		}

		[SerializeField]
		bool _dontDestroyOnLoad;

		/// <summary>
		/// Preserves compass bar between scene changes
		/// </summary>
		public bool dontDestroyOnLoad {
			get { return _dontDestroyOnLoad; }
			set { if (value!=_dontDestroyOnLoad) { _dontDestroyOnLoad = value; isDirty = true; } }
		}

		#endregion

		#region Events

		/// <summary>
		/// Event fired when this POI is visited.
		/// </summary>
		public event OnPOIVisited OnPOIVisited;
		/// <summary>
		/// Event fired when the POI appears in the compass bar (gets near than the visible distance)
		/// </summary>
		public event OnPOIVisible OnPOIVisible;
		/// <summary>
		/// Event fired when POI disappears from the compass bar (gets farther than the visible distance)
		/// </summary>
		public event OnPOIHide OnPOIHide;


		#endregion

		#region Public API

		/// <summary>
		/// Gets a reference to the Compass API.
		/// </summary>
		public static CompassPro instance {
			get {
				if (_instance == null) {
					GameObject o = GameObject.Find ("CompassNavigatorPro");
					if (o != null) {
						_instance = o.GetComponent<CompassPro> ();
					}
				}
				return _instance;
			}
		}

		/// <summary>
		/// Used to add a POI to the compass. Returns false if POI is already registered.
		/// </summary>
		public bool POIRegister(CompassProPOI newPOI) {
			for (int k=0;k<icons.Count;k++) {
				if (icons[k].poi==newPOI) {
					return false;
				}
			}
			CompassActiveIcon newIcon = new CompassActiveIcon(newPOI);
			icons.Add (newIcon);
			return true;
		}

		/// <summary>
		/// Returns whether the POI is currently registered.
		/// </summary>
		public bool POIisRegistered(CompassProPOI poi) {
			for (int k=0;k<icons.Count;k++) {
				if (icons[k].poi.id==poi.id) {
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Call this method to remove a POI from the compass.
		/// </summary>
		public void POIUnregister(CompassProPOI newPOI) {
			for (int k=0;k<icons.Count;k++) {
				if (icons[k].poi==newPOI) {
					if (icons[k].rectTransform!=null && icons[k].rectTransform.gameObject!=null) DestroyImmediate (icons[k].rectTransform.gameObject);
					icons.RemoveAt(k);
					break;
				}
			}
		}

		/// <summary>
		/// Shows given POI as gizmo in the scene and makes its icon always visible in the compass bar
		/// </summary>
		public void POIFocus(CompassProPOI existingPOI) {
			for (int k=0;k<icons.Count;k++) {
				if (icons[k].poi==existingPOI) {
					icons[k].poi.showPlayModeGizmo = true;
					icons[k].poi.clampPosition = true;
				} else if (icons[k].poi!=null) {
					icons[k].poi.showPlayModeGizmo = false;
					icons[k].poi.clampPosition = false;
				}
			}
		}

		/// <summary>
		/// Clears all gizmos and unfocus any focused POI.
		/// </summary>
		public void POIBlur() {
			for (int k=0;k<icons.Count;k++) {
				if (icons[k].poi!=null) {
					icons[k].poi.showPlayModeGizmo = false;
					icons[k].poi.clampPosition = false;
				}
			}
		}

		/// <summary>
		/// Show a light beacon over the specified POI.
		/// </summary>
		public void POIShowBeacon(CompassProPOI existingPOI, float duration) {
			Transform beacon = existingPOI.transform.Find("POIBeacon");
			if (beacon!=null) return;

			GameObject beaconObj = Instantiate(Resources.Load<GameObject>("Prefabs/POIBeacon"));
			beaconObj.name = "POIBeacon";
			beaconObj.hideFlags = HideFlags.DontSave;
			beacon = beaconObj.transform;
			beacon.position = existingPOI.transform.position + Vector3.up * beacon.transform.localScale.y * 0.5f;
			beacon.SetParent(existingPOI.transform, true);
			beacon.gameObject.GetComponent<BeaconAnimator>().duration = duration;

			if (audioSource!=null) {
				if (existingPOI.beaconAudioClip!=null) {
					audioSource.PlayOneShot(existingPOI.beaconAudioClip);
				} else if (_beaconDefaultAudioClip!=null) {
					audioSource.PlayOneShot(_beaconDefaultAudioClip);
				}
			}
		}


		/// <summary>
		/// Show a light beacon over all non-visited POIs.
		/// </summary>
		public void POIShowBeacon(float duration) {
			for (int k=0;k<icons.Count;k++) {
				CompassActiveIcon icon = icons[k];
				if (icon==null || icon.poi.isVisited || !icon.poi.isVisible) continue;
				POIShowBeacon(icon.poi, duration);
			}
		}

			
		/// <summary>
		/// Initiates a fade in effect with duration in seconds.
		/// </summary>
		public void FadeIn(float duration) {
			fadeDuration = duration;
			fadeStartTime = Time.time;
			prevAlpha = canvasGroup.alpha;
			alpha = 1f;
		}

		/// <summary>
		/// Initiates a fade out effect with duration in seconds.
		/// </summary>
		public void FadeOut(float duration) {
			fadeDuration = duration;
			fadeStartTime = Time.time;
			prevAlpha = canvasGroup.alpha;
			alpha = 0f;
		}


		public void ShowAnimatedText(string text) {
			StartCoroutine(AnimateDiscoverText(text));
		}

		#endregion
		
	}

}



