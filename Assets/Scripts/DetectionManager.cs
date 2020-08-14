using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    #region Singleton
    public static DetectionManager instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion

    public bool isNearDetected = false;
    public bool isBeingChased = false;
    public GameObject detectWarning;
    public GameObject chaseWarning;

   /* void Update()
    {
        *//*if (!chaseWarning.activeInHierarchy && isNearDetected)
        {
            detectWarning.SetActive(true);
        }
        else if (isBeingChased)
        {
            if (detectWarning.activeInHierarchy)
            {
                detectWarning.SetActive(false);
            }
            chaseWarning.SetActive(false);
        }
        else
        {
            detectWarning.SetActive(false);
            chaseWarning.SetActive(false);
        }*//*
        if(isNearDetected)
        {
            detectWarning.SetActive(true);
        }
        else
        {
            detectWarning.SetActive(false);
        }
        if (isBeingChased)
        {
            chaseWarning.SetActive(true);
        }
        else
        {
            chaseWarning.SetActive(false);
        }

    }*/
    public void SetDitection(bool state)
    {
        detectWarning.SetActive(state);
    }
    public void SetChase(bool state1)
    {
        chaseWarning.SetActive(state1);
    }
}
