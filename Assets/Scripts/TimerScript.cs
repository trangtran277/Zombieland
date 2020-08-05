using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] float startTime = 10f;
    // Start is called before the first frame update
    void Start()
    {
        startTime *= 60;
    }

    // Update is called once per frame
    void Update()
    {
        float timer = startTime - Time.timeSinceLevelLoad;
        if(timer > 0)
        {
            int minute = (int)timer / 60;
            int sec = (int)timer % 60;
            string displayMin = minute < 10 ? "0" + minute : "" + minute;
            string displaySec = sec < 10 ? "0" + sec : "" + sec;
            timerText.text = displayMin + " : " + displaySec;
        }
        else
        {
            timerText.GetComponent<Text>().color = Color.red;
        }
    }
}
