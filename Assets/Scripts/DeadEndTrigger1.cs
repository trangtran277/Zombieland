using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEndTrigger1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MissionManager.instance.isTriggerDeadEnd = true;
            Destroy(gameObject);
        }
    }
}
