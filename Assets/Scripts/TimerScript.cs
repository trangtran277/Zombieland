using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timer = Time.timeSinceLevelLoad;
        int minute = (int)timer / 60;
        int sec = (int)timer % 60;
        string displayMin = minute < 10 ? "0" + minute : "" + minute;
        string displaySec = sec < 10 ? "0" + sec : "" + sec;
        timerText.text = displayMin + " : " + displaySec;
    }
}
