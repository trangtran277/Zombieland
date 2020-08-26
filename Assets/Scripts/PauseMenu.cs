using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;

    public void OnButtonPause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnButtonResume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void OnButtonQuit()
    {
        Application.Quit();
    }

}
