using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTipTrigger : MonoBehaviour
{
    public GameObject firstTip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(firstTip != null)
            {
                firstTip.SetActive(true);
                StartCoroutine(DisableTip());
            }
            
        }
    }

    IEnumerator DisableTip()
    {
        yield return new WaitForSeconds(3f);
        Destroy(firstTip);
    }
}
