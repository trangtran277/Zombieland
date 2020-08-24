using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    

    public List<RectTransform> detectionPointers = new List<RectTransform>();
    private Dictionary<EnemyAI, int> chasingEnemies = new Dictionary<EnemyAI, int>();
    private AudioSource[] audios;

    private void Start()
    {
        audios = GetComponents<AudioSource>();
    }

    public void ActivateDetectionPointer(bool state, Transform enemy, Transform player, EnemyAI enemyAI)
    {
        if (state)
        {
            if(!chasingEnemies.ContainsKey(enemyAI))
            {
                if (chasingEnemies.Count < detectionPointers.Count)
                {
                    int index = 0;
                    while (detectionPointers[index].gameObject.activeSelf)
                        index++;
                    chasingEnemies.Add(enemyAI, index);
                    detectionPointers[index].gameObject.SetActive(true);
                    Vector3 pos;
                    float ang = Vector3.SignedAngle(Camera.main.transform.right, enemy.position - player.position, Vector3.up);
                    ang = 360 - ang;
                    pos.x = 300f * Mathf.Cos(ang * Mathf.Deg2Rad);
                    pos.y = 200f * Mathf.Sin(ang * Mathf.Deg2Rad);
                    pos.z = 0f;
                    detectionPointers[index].anchoredPosition = pos;
                    detectionPointers[index].rotation = Quaternion.Euler(new Vector3(0f, 0f, ang));
                }
            }
            else
            {
                int index = chasingEnemies[enemyAI];
                Vector3 pos;
                float ang = Vector3.SignedAngle(Camera.main.transform.right, enemy.position - player.position, Vector3.up);
                ang = 360 - ang;
                pos.x = 300f * Mathf.Cos(ang * Mathf.Deg2Rad);
                pos.y = 200f * Mathf.Sin(ang * Mathf.Deg2Rad);
                pos.z = 0f;
                detectionPointers[index].anchoredPosition = pos;
                detectionPointers[index].rotation = Quaternion.Euler(new Vector3(0f, 0f, ang));
            }
        }
        else
        {
            if (chasingEnemies.ContainsKey(enemyAI))
            {
                detectionPointers[chasingEnemies[enemyAI]].gameObject.SetActive(false);
                chasingEnemies.Remove(enemyAI);
            }
        }
            
    }

    void Update()
    {
        if (chasingEnemies.Count > 0)
        {
            isBeingChased = true;
            if (!audios[1].isPlaying)
            {
                audios[0].Pause();
                audios[1].Play();
            }

        }
        else
        {
            isBeingChased = false;
            if (!audios[0].isPlaying)
            {
                audios[0].Play();
                audios[1].Stop();
            }
        }
        if (isNearDetected && !isBeingChased)
        {
            detectWarning.SetActive(true);
        }
        else
        {
            detectWarning.SetActive(false);
        }
    }
}
