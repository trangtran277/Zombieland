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

    private void Update()
    {
        if (!chaseWarning.activeSelf && isNearDetected)
        {
            detectWarning.SetActive(true);
        }
        else if (isBeingChased)
        {
            if (detectWarning.activeSelf)
            {
                detectWarning.SetActive(false);
            }
            chaseWarning.SetActive(false);
        }
        else
        {
            detectWarning.SetActive(false);
            chaseWarning.SetActive(false);
        }
    }
}
