using System.Collections;
using System.Collections.Generic;
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
     void Start()
     {
          control = GetComponent<ThirdPersonUserControl>();
     }

     // Update is called once per frame
     void Update()
     {
          control.Hinput = leftJoystick.Horizontal;
          control.Vinput = leftJoystick.Vertical;

          cameraAngle += fixedTouchField.TouchDist.x * cameraAngleSpeed;

        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle,Vector3.up) * offset;
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
          
     }
}
