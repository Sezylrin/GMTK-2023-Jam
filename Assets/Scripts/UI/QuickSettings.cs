using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class QuickSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Toggle bgmToggle;
    public Toggle sfxToggle;
    public Button settingsButton;
    public GameObject settingsPanel;
    public bool bgmOn = true;
    public bool sfxOn = true;

    AudioSource buttonPress;

    void Awake()
    {
        buttonPress = GetComponent<AudioSource>();
        bgmToggle.isOn = bgmOn;
        sfxToggle.isOn = sfxOn;
    }

    public void ToggleBGM()
    {
        if (bgmToggle.isOn)
        {
            audioMixer.SetFloat("GameBGMVolume", 0);
            bgmOn = true;
            bgmToggle.GetComponent<Image>().color = new Color(255, 255, 255);
        }
        else
        {
            audioMixer.SetFloat("GameBGMVolume", -80);
            bgmOn = false;
            bgmToggle.GetComponent<Image>().color = new Color(1, 0.2f, 0.2f);
        }
        buttonPress.Play();
    }

    public void ToggleSFX()
    {
        if (sfxToggle.isOn)
        {
            audioMixer.SetFloat("GameSFXVolume", 0);
            sfxOn = true;
            sfxToggle.GetComponent<Image>().color = new Color(255, 255, 255);
        }
        else
        {
            audioMixer.SetFloat("GameSFXVolume", -80);
            sfxOn = false;
            sfxToggle.GetComponent<Image>().color = new Color(1, 0.2f, 0.2f);
        }
        buttonPress.Play();
    }

    public void OpenSettings()
    {
        if (settingsPanel.activeSelf && settingsButton != null) 
        {
            settingsPanel.SetActive(false);
        } else {
            settingsPanel.SetActive(true);
        }
        buttonPress.Play();
    }
    
}
