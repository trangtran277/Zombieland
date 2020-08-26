using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion

    public GameObject gameOverUI;
    public bool gameEnded = false;

    public void ActivateGameOverUI()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            StartCoroutine(DelayActivate());
        }
    }

    IEnumerator DelayActivate()
    {
        yield return new WaitForSeconds(3f);
        gameOverUI.SetActive(true);
    }

    public void OnButtonRestart()
    {
        SceneManager.LoadScene("AudioPlayer");
    }

    public void OnButtonExit()
    {
        Application.Quit();
    }
}
