using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;

    public AudioClip coinPickup;


    private bool isMuted = false; 
    private float lastVolume = 0.01f; 

    public float Volume
    {
        get => isMuted ? 0f : lastVolume;  
        set
        {
            if (audioSource != null)
            {
                lastVolume = Mathf.Clamp01(value);  
                if (!isMuted)  
                {
                    audioSource.volume = lastVolume;
                }
            }
        }
    }

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

    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

       

        audioSource.loop = true;

        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.volume = isMuted ? 0f : lastVolume;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void PlayCoinPickupSound()
    {
        if (coinPickup != null)
        {
            audioSource.PlayOneShot(coinPickup);
        }
    }

}
