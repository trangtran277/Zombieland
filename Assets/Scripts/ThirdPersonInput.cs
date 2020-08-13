using Cinemachine;
using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonInput : MonoBehaviour
{
     // Start is called before the first frame update
     public FixedJoystick leftJoystick;
     public FixedTouchField fixedTouchField;
     protected ThirdPersonUserControl control;

     protected float cameraAngle;
     protected float cameraAngleSpeed = .2f;
     public Vector3 offset = new Vector3(0f, 2f, 4f);

    //public EquipmentManager equipmentManager;
    public GameObject riu;

    //public GameObject hearbar;
    //private Slider sliderHealth;
    public HealthBar healthBar;
    private Animator animatorThirdperson;
    private float healthCharacter;
    public GameObject cam;
    private CinemachineFreeLook cinemachineFreeLook;
    private Transform transformCinemachineFreeLook;

     void Start()
     {
         control = GetComponent<ThirdPersonUserControl>();
        //sliderHealth = hearbar.GetComponent<Slider>();
        animatorThirdperson = GetComponent<Animator>();
        
        healthCharacter = GetComponent<ThirdPersonCharacter>().health;
        healthBar.SetMaxHealth(healthCharacter);
        cinemachineFreeLook = cam.GetComponent<CinemachineFreeLook>();
        transformCinemachineFreeLook = cam.GetComponent<Transform>();
     }

     // Update is called once per frame
     void Update()
     {
        /*if (EquipmentManager.instance.currentEquipment[3] != null)
        {
            riu.SetActive(true);
        }
        else
        {
            riu.SetActive(false);
        }*/

        

        control.Hinput = leftJoystick.Horizontal;
          control.Vinput = leftJoystick.Vertical;

          cameraAngle += fixedTouchField.TouchDist.x * cameraAngleSpeed;

        /*transformCinemachineFreeLook.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * offset;*/
        cinemachineFreeLook.m_XAxis.Value = cameraAngle;

        /*Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * offset;
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
*/
        healthCharacter = GetComponent<ThirdPersonCharacter>().health;
        //sliderHealth.value = healthCharacter;
        healthBar.SetHealth(healthCharacter);
        if (healthCharacter<=0)
        {
            animatorThirdperson.SetTrigger("die");
            StartCoroutine(WaitToSetActiveFalse());

        }
     }
    IEnumerator WaitToSetActiveFalse()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);

    }

}
