using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject optionMenuUI;
    public GameObject mainMenuUI;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenOptions()
    {
        mainMenuUI.SetActive(false);
        optionMenuUI.SetActive(true);
    }

    public void Back()
    {
        optionMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        //Debug.Log("Quit");
        Application.Quit();
    }
}
