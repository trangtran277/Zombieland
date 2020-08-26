using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject loadingUI;
    public void LoadLevel()
    {
        Debug.Log("Attempt to load level?");
        mainMenuUI.SetActive(false);
        //loadingUI.SetActive(true);
        StartCoroutine("LoadAsynchronously");
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Running coroutine");
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return null;
        }
    }
}
