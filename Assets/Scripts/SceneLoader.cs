using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    enum Scenes
    {
        MainMenu = 0,
        Game = 1
        //Add other scenes here
    }
    
    public static SceneLoader instance;
    public GameObject loadingScreen;

    private void Awake() 
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }

    //load scene single
    public void LoadSceneIndex(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    //load async method with loading screen
    public void LoadSceneIndexAsync(int index)
    {
        StartCoroutine(LoadSceneAsync(index));
    }

    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            yield return null;
        }
        loadingScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

 
}
