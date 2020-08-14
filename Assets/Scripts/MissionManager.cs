using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public GameObject Gate1;
    public GameObject Gate2;
    public Text curMission;
    bool gate1Completed = false;
    bool gate2Completed = false;

    private void Start()
    {
       
        curMission.text = "Open the North Gate";
    }

    private void Update()
    {
        if (!gate1Completed && Gate1.GetComponent<ThingsToInteract>().completed)
        {
            Transform[] doorGate1 = Gate1.GetComponentsInChildren<Transform>();
            doorGate1[1].Rotate(0f, 90f, 0f);
            doorGate1[2].Rotate(0f, -90f, 0f);
            Gate1.GetComponent<BoxCollider>().enabled = false;
            curMission.text = "Open the South Gate";
            gate1Completed = true;
        }
        if (!gate2Completed && Gate2.GetComponent<ThingsToInteract>().completed)
        {
            Transform[] doorGate2 = Gate2.GetComponentsInChildren<Transform>();
            doorGate2[1].Rotate(0f, 90f, 0f);
            doorGate2[2].Rotate(0f, -90f, 0f);
            Gate2.GetComponent<BoxCollider>().enabled = false;
            gate2Completed = true;
            StartCoroutine(ExitGame());
        }
    }

    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1.5f);
        //Debug.Log("exit");
        Application.Quit();
    }
}
