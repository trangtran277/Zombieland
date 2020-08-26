using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Text timerText;
    //[SerializeField] Transform player;
    [SerializeField] GameObject enemy;
    [SerializeField] int spawnNumber = 20;
    [SerializeField] List<Transform> spawnPos = new List<Transform>();
    [SerializeField] float radius;
    public LightingManager lighting;
    public GameObject nightWarning;

    private bool hasSpawned = false;
    private bool hasRemoved = false;


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
                GetComponent<AudioSource>().Play();
                Spawn();
                hasSpawned = true;
            }
        } 
        else
        {
            timerText.color = Color.black;
            if (hasSpawned && !hasRemoved)
            {
                Remove();
                hasRemoved = true;
            }
        }
    }
    public void Spawn()
    {
        for (int j = 0; j < spawnPos.Count; j++)
        {
            for (int i = 0; i < spawnNumber; i++)
            {
                GameObject nightEnemy = ObjectPooler.SharedInstance.GetPooledObject();
                if (nightEnemy != null)
                {
                    nightEnemy.transform.position = RandomizePosition(spawnPos[j]);
                    nightEnemy.transform.rotation = Quaternion.identity;
                    nightEnemy.SetActive(true);
                }
            }
        }
    }

    public void Remove()
    {
        List<GameObject> objectPool = ObjectPooler.SharedInstance.pooledObjects;
        foreach (GameObject obj in objectPool)
        {
            obj.SetActive(false);
        }
    }

    private Vector3 RandomizePosition(Transform centerPos)
    {
        float x = centerPos.position.x + Random.Range(- radius, radius);
        float z = centerPos.position.z + Random.Range(- radius, radius);
        return new Vector3(x, 0f, z);
    }
}
