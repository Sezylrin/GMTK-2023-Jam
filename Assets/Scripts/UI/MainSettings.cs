using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainSettings : MonoBehaviour
{
    public static MainSettings instance;
    public AudioMixer audioMixer;
    public Slider BGMSlider;
    public Slider SFXSlider;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BGMSliderValue()
    {
        float newVolume =(80f - (BGMSlider.value * 80f)) * -1;
        audioMixer.SetFloat("GameBGM", newVolume);
    }

    public void SFXSliderValue()
    {
        float newVolume = (80f - (SFXSlider.value * 80f)) * -1;
        audioMixer.SetFloat("GameBGM", newVolume);
    }
}
