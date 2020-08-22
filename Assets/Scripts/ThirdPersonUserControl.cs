using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.ThirdPerson
{

     [RequireComponent(typeof(ThirdPersonCharacter))]
     public class ThirdPersonUserControl : MonoBehaviour
     {
          public GameObject collectButton;
          public float interactionRadius = 1f;
          public float findEnemyRadius = 13f;
          public ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
          private Transform m_Cam;                  // A reference to the main camera in the scenes transform
          private Vector3 m_CamForward;             // The current forward direction of the camera
          private Vector3 m_Move;
          public bool m_Jump;
          [HideInInspector]
          public float Vinput, Hinput;                    // the world-relative desired move direction, calculated from the camForward and user input.
                                                          //public UiManagerController UIcontroller;
          public bool crouchswt;
          private void Start()
          {
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
            // read inputs
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            //float v = CrossPlatformInputManager.GetAxis("Vertical");
            //bool crouch = Input.GetKey(KeyCode.C);
                //bool crouch = crouchswt;

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
            //if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            //m_Character.Move(m_Move, crouch, m_Jump);
            m_Character.Move(m_Move, false, false);
            //m_Jump = false;

            //CheckItemAround();
               CheckEnemyAround();

               
          }

          public IInteractable CheckItemAround()
          {
               Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
               foreach (Collider hit in hits)
               {
                    IInteractable interactable = hit.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                         collectButton.SetActive(true);
                         Vector3 pos = Camera.main.WorldToScreenPoint(hit.transform.position);
                         collectButton.transform.position = new Vector3(pos.x, pos.y + 15f, pos.z);
                         if (interactable is NoteObject)
                         {
                             collectButton.GetComponentInChildren<Text>().text = "(Tap) Read";
                         }
                         else if (interactable is Bed)
                         {
                             collectButton.GetComponentInChildren<Text>().text = "(Tap) Sleep";
                         }
                         else if (interactable is ThingsToInteract)
                         {
                             collectButton.GetComponentInChildren<Text>().text = "(Tap) Interact";
                         }
                         else
                         {
                             collectButton.GetComponentInChildren<Text>().text = "(Tap) Collect";
                         }
                         return interactable;
                    }
               }

               collectButton.SetActive(false);
               return null;
          }

          public void CheckEnemyAround()
          {
            Collider[] hits = Physics.OverlapSphere(transform.position, findEnemyRadius);
            foreach (Collider hit in hits)
            {
                EnemyAI enemy = hit.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.FindTarget();
                }
            }
          }
     }
}
