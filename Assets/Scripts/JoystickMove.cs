using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
     // Start is called before the first frame update
     public FixedJoystick joystick;
     private PlayMakerFSM playMakerFSM;
     void Start()
     {
          playMakerFSM = GetComponent<PlayMakerFSM>();
     }
     public void SetJoyStick()
     {

     }
     void Update()
    {
          Debug.Log(joystick.Vertical);
     }
}
