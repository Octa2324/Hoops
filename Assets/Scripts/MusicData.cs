using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicData : MonoBehaviour
{
    public static MusicData Instance;

    public int SoundEffectsMute = 0;
    public int MusicMute = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
