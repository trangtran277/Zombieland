using UnityEngine;
using System.Collections;

namespace CompassNavigatorPro
{
	[AddComponentMenu("Compass Navigator Pro/Compass POI")]
	[ExecuteInEditMode]
	public class CompassProPOI : MonoBehaviour
	{
		[Tooltip("Title to be shown when this POI is in the center of the compass bar and it's a known location (isVisited = true)")]
		public string title;

		[Tooltip("Specifies if this POI is visible in the compass bar")]
		public bool isVisible;

		[Tooltip("Specifies if POI must be removed from compass bar when visited.")]
		public bool hideWhenVisited;

		[Tooltip("Specifies if this POI has been already visited.")]
		public bool isVisited;

		[Tooltip("Text to show when discovered. Leave this to null if you don't want to show any text.")]
		public string visitedText;
		
		[Tooltip("Sound to play when POI is visited the first time.")]
		public AudioClip visitedAudioClip;

		[Tooltip("Radius of activity of this POI. Useful for area POIs.")]
		public float radius;

		[Tooltip("The icon for the POI if has not been discovered/visited.")]
		public Sprite iconNonVisited;

		[Tooltip("The icon for the POI if has been visited.")]
		public Sprite iconVisited;
		
		[Tooltip("If the icon will be shown in the scene during playmode. If enabled, the icon will fade in smoothly as the player approaches it.")]
		public bool showPlayModeGizmo;
		
		[Tooltip("If enabled, the icon will stop at the edges of the bar even if it's behind the player.")]
		public bool clampPosition;

		[HideInInspector]
		public float distanceToCameraSQR;

		[HideInInspector]
		public Vector3 iconScale;

		[HideInInspector]
		public float iconAlpha;

		[HideInInspector]
		public SpriteRenderer spriteRenderer;

		[Tooltip("Sound to play when beacon is shown.")]
		public AudioClip beaconAudioClip;

		[Tooltip("Preserves the state of this POI between scene changes. Note that this POI only will be visible in the scene where it was first created.")]
		public bool dontDestroyOnLoad;

		[Tooltip("Unique ID to be used when DontDestroyOnLoad is set to true. Otherwise it's not used.")]
		[HideInInspector]
		public int id;

		void OnEnable() {
			if (id==0) {
				id = System.Guid.NewGuid().GetHashCode();
			}
		}

		// Use this for initialization
		void Start ()
		{
			CompassPro compass = CompassPro.instance;
			if (compass==null) return;

			if (dontDestroyOnLoad && Application.isPlaying) {
				if (compass.POIisRegistered(this)) {
					Destroy (gameObject);
					return;
				}
				DontDestroyOnLoad(gameObject);
			}

			compass.POIRegister (this);
		}

		void OnDrawGizmos() {
			Gizmos.DrawIcon(transform.position, "compassIcon.png", true);
		}

	
	}

}