using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.Characters.ThirdPerson
{

	 [RequireComponent(typeof(ThirdPersonCharacter))]
	 public class ThirdPersonUserControl : MonoBehaviour
	 {
		  public GameObject interactionCircle;
		  public float interactionRadius = 0.5f;
		  public ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
		  private Transform m_Cam;                  // A reference to the main camera in the scenes transform
		  private Vector3 m_CamForward;             // The current forward direction of the camera
		  private Vector3 m_Move;
		  public bool m_Jump;
		  [HideInInspector]
		  public float Vinput, Hinput;                    // the world-relative desired move direction, calculated from the camForward and user input.
		  //public UiManagerController UIcontroller;

		  private void Start()
		  {
			   //Cursor.lockState = CursorLockMode.Locked;
			   // get the transform of the main camera
			   if (Camera.main != null)
			   {
					m_Cam = Camera.main.transform;
			   }
			   else
			   {
					Debug.LogWarning(
						 "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
					// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
			   }

			   // get the third person character ( this should never be null due to require component )
			   m_Character = GetComponent<ThirdPersonCharacter>();
		  }


		  private void Update()
		  {
			   if (!m_Jump)
			   {
					m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			   }
		  }


		  // Fixed update is called in sync with physics
		  private void FixedUpdate()
		  {
			   /*if(EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}*/
			   // read inputs
			   //float h = CrossPlatformInputManager.GetAxis("Horizontal");
			   //float v = CrossPlatformInputManager.GetAxis("Vertical");
			   bool crouch = Input.GetKey(KeyCode.C);

			   // calculate move direction to pass to character
			   if (m_Cam != null)
			   {
					// calculate camera relative direction to move:
					m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
					m_Move = Vinput * m_CamForward + Hinput * m_Cam.right;
			   }
			   else
			   {
					// we use world-relative directions in the case of no main camera
					m_Move = Vinput * Vector3.forward + Hinput * Vector3.right;
			   }
#if !MOBILE_INPUT
			   // walk speed multiplier
			   if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

			   // pass all parameters to the character control script
			   m_Character.Move(m_Move, crouch, m_Jump);
			   m_Jump = false;

			   CheckItemAround();
			   CheckEnemyAround();

			   
		  }

		  public Interactable CheckItemAround()
		  {
			   Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
			   foreach (Collider hit in hits)
			   {
					Interactable interactable = hit.GetComponent<Interactable>();
					if (interactable != null)
					{
						 float objectHeight = 0f;
						 MeshFilter mesh = hit.GetComponent<MeshFilter>();
						 if(mesh != null)
						 {
							objectHeight = mesh.sharedMesh.bounds.size.y * hit.transform.localScale.y;
						 }
						 
						 interactionCircle.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y - objectHeight/2 + 0.005f, hit.transform.position.z);
						 interactionCircle.SetActive(true);
						 return interactable;
					}
			   }

			   if (interactionCircle.activeSelf)
			   {
					interactionCircle.SetActive(false);
			   }
			   return null;
		  }

		  public EnemyController CheckEnemyAround()
          {
			float attackRange = m_Character.attackRange;
			float damage = m_Character.baseDamge;
			Equipment weapon = EquipmentManager.instance.currentEquipment[(int)EquipmentSlot.Weapon];
			if (weapon != null)
			{
				attackRange += weapon.equipmentItem.range;
				damage += weapon.equipmentItem.damage;
			}
			Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
			foreach (Collider hit in hits)
			{
				//Debug.Log("Attacked");
				EnemyController enemy = hit.GetComponent<EnemyController>();
				if (enemy != null)
				{
					return enemy;
				}
			}
			return null;
		  }
	 }
}
