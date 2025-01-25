using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource src;
    public AudioClip music;

    private bool isMuted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        src = GetComponent<AudioSource>();
        if (src == null)
        {
            src = gameObject.AddComponent<AudioSource>();
        }

        src.clip = music;
        src.loop = true;
        src.playOnAwake = false;

        LoadMuteState();
        UpdateMuteState();

        if (music != null)
        {
            src.Play();
        }
    }



    public void ToggleMute()
    {
        isMuted = !isMuted;
        UpdateMuteState();
        SaveMuteState();
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    private void UpdateMuteState()
    {
        if (src != null)
        {
            src.mute = isMuted;
        }
    }

    private void SaveMuteState()
    {
        MusicData.Instance.MusicMute = isMuted ? 1 : 0;
    }

    private void LoadMuteState()
    {
        isMuted = MusicData.Instance.MusicMute == 1;
    }

}
