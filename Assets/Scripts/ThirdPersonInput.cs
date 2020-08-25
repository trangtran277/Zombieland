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
     public AudioSource[] audioSources;
     public FixedJoystick leftJoystick;
     public FixedTouchField fixedTouchField;
     protected ThirdPersonUserControl control;

     protected float cameraAngle;
     protected float cameraAngleSpeed = .5f;
     //public Vector3 offset = new Vector3(0f, 2f, 4f);

    //public EquipmentManager equipmentManager;
    public GameObject riu;

    //public GameObject hearbar;
    //private Slider sliderHealth;
    public HealthBar healthBar;
    private Animator animatorThirdperson;
    private float healthCharacter;

    bool healthLower = false;
    bool isAlive = true;
    //public GameObject cam;
    //private CinemachineFreeLook cinemachineFreeLook;
    //private Transform transformCinemachineFreeLook;
    private void Awake()
    {
        
    }
    void Start()
     {
         control = GetComponent<ThirdPersonUserControl>();
        //sliderHealth = hearbar.GetComponent<Slider>();
        animatorThirdperson = GetComponent<Animator>();
        
        //healthCharacter = GetComponent<ThirdPersonCharacter>().health;
        healthBar.SetMaxHealth(GetComponent<ThirdPersonCharacter>().maxhealth);
        //healthCharacter = 60;
        //cinemachineFreeLook = cam.GetComponent<CinemachineFreeLook>();
        //transformCinemachineFreeLook = cam.GetComponent<Transform>();
    }

     // Update is called once per frame
     void Update()
     {
        if (EquipmentManager.instance.currentEquipment[3] != null)
        {
            riu.SetActive(true);
        }
        else
        {
            riu.SetActive(false);
        }
       // Debug.Log(enemys.Length);
        /*foreach(GameObject e in enemys)
        {
            if (e.GetComponent<EnemyControlPatrolPath>() != null)
            {
                Debug.Log(e.GetComponent<EnemyControlPatrolPath>().detectionDistance);
                Debug.Log(e.GetComponent<EnemyControlPatrolPath>().fieldOfVision);
            }
            else
            {
                Debug.Log(e.GetComponent<EnemyController>().detectionDistance);
                Debug.Log(e.GetComponent<EnemyController>().fieldOfVision);
            }
        }*/
        
        

        control.Hinput = leftJoystick.Horizontal * 1.5f;
        control.Vinput = leftJoystick.Vertical * 1.5f;

          cameraAngle += fixedTouchField.TouchDist.x * cameraAngleSpeed;

        //transformCinemachineFreeLook.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * offset;
        //cinemachineFreeLook.m_XAxis.Value = cameraAngle;

        /*Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * offset;
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
*/
        healthCharacter = GetComponent<ThirdPersonCharacter>().health;
        //sliderHealth.value = healthCharacter;
        healthBar.SetHealth(healthCharacter);
        if (healthCharacter<=0)
        {
            healthLower = false;
            if(isAlive)
            {
                audioSources[3].Play();
                StartCoroutine(WaitToGameOver());
                isAlive = false;
            }
            
            animatorThirdperson.SetTrigger("die");

            StartCoroutine(WaitToSetActiveFalse());

        }else if(healthCharacter<=30 && healthCharacter>0 && !healthLower)
        {
            healthLower = true;
            StartCoroutine(WaitToHeart());
        }
          
     }
    IEnumerator WaitToSetActiveFalse()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);

    }
    IEnumerator WaitToGameOver()
    {
        yield return new WaitForSeconds(2f);
        audioSources[2].Play();
    }
    IEnumerator WaitToHeart()
    {
        audioSources[1].Play();
        yield return new WaitUntil(() => healthCharacter>30);
        audioSources[1].Stop();
        healthLower = false;
    }
}
