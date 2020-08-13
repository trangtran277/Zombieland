using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] float startNightTime = 10f;
    [SerializeField] Transform player;
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnNumber = 10f;
    [SerializeField] Transform centerPos;
    [SerializeField] float radius;

    private bool hasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        startNightTime *= 60;
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

        if(timer >= startNightTime)
        {
            if(!hasSpawned)
            {
                Spawn();
                hasSpawned = true;
            }
        }
    }
    public void Spawn()
    {
        for (int i = 0; i < spawnNumber; i++)
        {
            //GameObject newEnemy = Instantiate(enemy, RandomizePosition(), Quaternion.identity);
            //newEnemy.GetComponent<EnemyController>().player = player.transform;
            GameObject pickUp = ObjectPooler.SharedInstance.GetPooledObject();
            if (pickUp != null)
            {
                pickUp.transform.position = RandomizePosition();
                pickUp.transform.rotation = Quaternion.identity;
                pickUp.SetActive(true);
            }
        }
    }

    private Vector3 RandomizePosition()
    {
        float x = centerPos.position.x + Random.Range(- radius, radius);
        float z = centerPos.position.z + Random.Range(- radius, radius);
        return new Vector3(x, 0f, z);
    }
}
