using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    public static AudioEffects Instance;

    private AudioSource src;
    public AudioClip pickUp, boing;

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

        LoadMuteState();
        UpdateMuteState();
    }

    public void PickUpCoin()
    {
        if (!isMuted && src != null && pickUp != null)
        {
            src.PlayOneShot(pickUp);
        }
    }

    public void Boing()
    {
        if (!isMuted && src != null && boing != null)
        {
            src.PlayOneShot(boing);
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
        PlayerPrefs.SetInt("MuteState", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadMuteState()
    {
        isMuted = PlayerPrefs.GetInt("MuteState", 0) == 1;
    }
}
