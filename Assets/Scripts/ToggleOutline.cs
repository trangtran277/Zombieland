using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOutline : MonoBehaviour
{
    cakeslice.Outline outline;
    public float maxRange = 50f;
    RaycastHit hit;

    private void Start()
    {
        outline = GetComponent<cakeslice.Outline>();
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < maxRange)
        {
            if (Physics.Raycast(transform.position, (Camera.main.transform.position - transform.position), out hit, maxRange))
            {
                Debug.DrawLine(transform.position, Camera.main.transform.position, Color.red);
                if (hit.transform.CompareTag("Camera") || hit.transform.CompareTag("Player"))
                {
                    outline.enabled = true;
                }
                else
                {
                    outline.enabled = false;
                }
            }
        }
        else
        {
            outline.enabled = false;
        }
    }
}
