using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public int currentTrack;

    public TextMeshProUGUI trackNameText;

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

    private void Start()
    {
        audioSource.clip = audioClips[currentTrack];
        audioSource.Play();
        ChangeTrackPlaying();
    }

    public void PlayTrack(int track)
    {
        if (track < audioClips.Length)
        {
            audioSource.clip = audioClips[track];
            audioSource.Play();
            currentTrack = track;
            ChangeTrackPlaying();
        }
    }
    private void Update()
    {
        
    }
    public void StopTrack()
    {
        audioSource.Stop();
    }

    public void ChangeTrackPlaying()
    {
        if (trackNameText != null)
            trackNameText.text = "Now playing... " + audioClips[currentTrack].name;
    }

}
