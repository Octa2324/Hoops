using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    public static AudioEffects Instance;

    private AudioSource src;
    public AudioClip pickUp, boing, select, ballHit;

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

    public void Select()
    {
        if (!isMuted && src != null && select != null)
        {
            src.PlayOneShot(select);
        }
    }

    public void BallHit()
    {
        if (!isMuted && src != null && ballHit != null)
        {
            src.PlayOneShot(ballHit);
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
        MusicData.Instance.SoundEffectsMute = isMuted ? 1 : 0;
    }

    private void LoadMuteState()
    {
        isMuted = MusicData.Instance.SoundEffectsMute == 1;
    }
}
