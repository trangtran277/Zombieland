using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterCutsceneTrigger : MonoBehaviour
{
    public void LoadMenu()
    {
        StartCoroutine(ShowMenu());
    }

    IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
    }
}
