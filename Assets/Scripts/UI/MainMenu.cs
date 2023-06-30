using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    Button playButton;
    Button settingsButton;
    Button quitButton;

     //Button Names - "Play", "Settings", "Quit,

    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("Play");
        settingsButton = root.Q<Button>("Settings");
        quitButton = root.Q<Button>("Quit");

        playButton.clicked += Play;
        settingsButton.clicked += Settings;
        quitButton.clicked += Quit;
    }

    private void Play()
    {
        playButton.clicked += () => Debug.Log("Play");
        //Load Game Scene
    }

    private void Settings()
    {
        settingsButton.clicked += () => Debug.Log("Settings");
        //Open settings menu
    }

    private void Quit()
    {
        quitButton.clicked += () => Debug.Log("Quit");
        
        
        
        Application.Quit();
    }
}
