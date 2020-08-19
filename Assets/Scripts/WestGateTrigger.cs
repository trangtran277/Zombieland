using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WestGateTrigger : MonoBehaviour
{
    public MissionManager missionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            missionManager.isTriggerWestGate = true;
        }
    }
}
