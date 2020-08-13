using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public LightingManager lighting;
    float curTime;
    public GameObject stopInput;
    public void Interact()
    {
        Time.timeScale = 50f;
        stopInput.SetActive(true);
    }

    void Update()
    {
        curTime = lighting.TimeOfDay;
        if(curTime > 4.85f && curTime <=5f) 
        {
            if(Time.timeScale > 1f)
            {
                Time.timeScale = 1f;
                //lighting.TimeOfDay = 5f;
                stopInput.SetActive(false);
            }
        }
    }
}
