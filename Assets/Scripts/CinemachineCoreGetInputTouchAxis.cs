using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinemachineCoreGetInputTouchAxis : MonoBehaviour
{

    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;
    public FixedTouchField fixedTouchField;
    //float ScreenWidth;

    // Use this for initialization
    void Start()
    {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
        //ScreenWidth = Screen.width/2;
    }

    float HandleAxisInputDelegate(string axisName)
    {
        switch (axisName)
        {

            case "Mouse X":
                if (Mathf.Abs(fixedTouchField.TouchDist.x) > 0.01)
                    return fixedTouchField.TouchDist.x / TouchSensitivity_x;
                else
                    return 0f;
            /*if (Input.touchCount > 0)
            {
                for(int i=0;i<Input.touchCount;i++)
                {
                    if(Camera.main.ScreenToWorldPoint(Input.touches[i].position).x > ScreenWidth)
                    {
                        return fixedTouchField.TouchDist.x / TouchSensitivity_x;
                    }
                }
                return Input.GetAxis(axisName);
                // return Input.touches[0].deltaPosition.x / TouchSensitivity_x;
            }
            else
            {
                return Input.GetAxis(axisName);
            }*/

            case "Mouse Y":
                if (Mathf.Abs(fixedTouchField.TouchDist.y) > 0.01)
                    return fixedTouchField.TouchDist.y / TouchSensitivity_y;
                else
                    return 0f;
            /*if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Camera.main.ScreenToWorldPoint(Input.touches[i].position).x > ScreenWidth)
                    {
                        return fixedTouchField.TouchDist.y / TouchSensitivity_y;
                    }
                }
                return Input.GetAxis(axisName);
                //return Input.touches[0].deltaPosition.y / TouchSensitivity_y;
            }
            else
            {
                return Input.GetAxis(axisName);
            }*/

            default:
                Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                break;
        }

        return 0f;
    }
}
