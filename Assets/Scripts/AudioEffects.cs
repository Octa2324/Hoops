using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    public AudioSource src;
    public AudioClip pickUp;

    private static AudioEffects instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickUpCoin()
    {
        if (src != null && pickUp != null)
        {
            src.PlayOneShot(pickUp);
        }
    }
}
