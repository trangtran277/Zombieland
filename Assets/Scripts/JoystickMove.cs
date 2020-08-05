using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
     // Start is called before the first frame update
     public Joystick joystick;
     private PlayMakerFSM playMakerFSM;
     void Start()
     {
          playMakerFSM = GetComponent<PlayMakerFSM>();
     }
     public void SetJoyStick()
     {

     }
     //  void Update()
     //  {
     //       Debug.Log(joystick.Horizontal);
     //       Debug.Log(joystick.Vertical);
     //       playMakerFSM.FsmVariables.GetFsmFloat("Input mag").Value = Mathf.Abs(Mathf.Max(joystick.Horizontal, joystick.Vertical));
     //       playMakerFSM.FsmVariables.GetFsmVector3("Input vector").Value = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
     //       Debug.Log(playMakerFSM.FsmVariables.GetFsmFloat("Input mag").Value);
     //       Debug.Log(playMakerFSM.FsmVariables.GetFsmVector3("Input vector").Value);
     //  }
}
