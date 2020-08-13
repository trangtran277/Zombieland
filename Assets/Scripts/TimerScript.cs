using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Text timerText;
    //[SerializeField] float startNightTime = 10f;
    [SerializeField] Transform player;
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnNumber = 10f;
    [SerializeField] Transform centerPos;
    [SerializeField] float radius;
    public LightingManager lighting;
    public GameObject nightWarning;

    private bool hasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        //startNightTime *= 60;
    }

    // Update is called once per frame
    void Update()
    {
        float curTime = lighting.TimeOfDay;
        int hour = (int)curTime;
        int minute = (int)((curTime-(int)curTime)*60);
        string displayHour = hour < 10 ? "0" + hour : "" + hour;
        string displayMin = minute < 10 ? "0" + minute : "" + minute;
        timerText.text = displayHour + " : " + displayMin;
        if (curTime >= 18 && curTime < 19)
            nightWarning.SetActive(true);
        else
            nightWarning.SetActive(false);

        if (curTime >= 19 || curTime < 5)
        {
            timerText.color = Color.red;
            if (!hasSpawned)
            {
                Spawn();
                hasSpawned = true;
            }
        }
        else
        {
            timerText.color = Color.black;
        }
        /*float timer = Time.timeSinceLevelLoad;
        int minute = (int)timer / 60;
        int sec = (int)timer % 60;
        string displayMin = minute < 10 ? "0" + minute : "" + minute;
        string displaySec = sec < 10 ? "0" + sec : "" + sec;
        timerText.text = displayMin + " : " + displaySec;

        if(timer >= startNightTime)
        {
            if(!hasSpawned)
            {
                Spawn();
                hasSpawned = true;
            }
        }*/
    }
    public void Spawn()
    {
        for (int i = 0; i < spawnNumber; i++)
        {
            GameObject newEnemy = Instantiate(enemy, RandomizePosition(), Quaternion.identity);
            newEnemy.GetComponent<EnemyController>().player = player.transform;
         
        }
    }

    private Vector3 RandomizePosition()
    {
        float x = centerPos.position.x + Random.Range(- radius, radius);
        float z = centerPos.position.z + Random.Range(- radius, radius);
        return new Vector3(x, 0f, z);
    }
}
