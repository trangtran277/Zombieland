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
	 //public FixedTouchField fixedTouchField;
	 protected ThirdPersonUserControl control;

	 protected float cameraAngle;
	 protected float cameraAngleSpeed = .2f;
	//public Vector3 offset = new Vector3(0f, 2f, 4f);
	 //public Vector3 offset;


	//public GameObject hearbar;
	//private Slider sliderHealth;
	public HealthBar healthBar;
	private Animator animatorThirdperson;
	private float healthCharacter;
	 void Start()
	 {
		control = GetComponent<ThirdPersonUserControl>();
		//sliderHealth = hearbar.GetComponent<Slider>();
		animatorThirdperson = GetComponent<Animator>();
		
		healthCharacter = GetComponent<ThirdPersonCharacter>().health;
		healthBar.SetMaxHealth(healthCharacter);
		//offset = Camera.main.transform.position - transform.position;
	 }

	 // Update is called once per frame
	 void Update()
	 {
		control.Hinput = leftJoystick.Horizontal * 1.5f;
		control.Vinput = leftJoystick.Vertical * 1.5f;
		  /*if(leftJoystick.Horizontal > 0)
		  {
			control.Hinput = 0.3f + leftJoystick.Horizontal;
		  }
		  else if(leftJoystick.Horizontal < 0)
          {
			control.Hinput = -0.3f + leftJoystick.Horizontal;
          }
		  if (leftJoystick.Vertical > 0)
		  {
			control.Vinput = 0.3f + leftJoystick.Vertical;
		  }
		  else if (leftJoystick.Vertical < 0)
		  {
			control.Vinput = -0.3f + leftJoystick.Vertical;
		  }*/

		  //cameraAngle += fixedTouchField.TouchDist.x * cameraAngleSpeed;

		//Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle,Vector3.up) * offset;
		  //Camera.main.transform.position = transform.position + offset;
		//Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
		  //Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
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
