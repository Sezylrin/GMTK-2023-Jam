using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public GameObject loadingScreen;

    private void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadSceneIndex(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    enum Scenes
    {
        MainMenu = 0,
        Game = 1
        //Add other scenes here
    }
}
