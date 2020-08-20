using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTipTrigger : MonoBehaviour
{
    public GameObject secondTip;
    bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (secondTip != null && activated == false)
            {
                secondTip.SetActive(true);
                activated = true;
                StartCoroutine(DisableTip());
            }

        }
    }

    IEnumerator DisableTip()
    {
        yield return new WaitForSeconds(3f);
        secondTip.SetActive(false);
    }
}
