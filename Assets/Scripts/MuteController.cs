using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteController : MonoBehaviour
{
    public Button muteButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private void Start()
    {
        if (muteButton != null)
        {
            muteButton.onClick.AddListener(ToggleMute);
            UpdateButtonSprite();
        }
    }

    private void ToggleMute()
    {
        if (AudioEffects.Instance != null)
        {
            AudioEffects.Instance.ToggleMute();
            UpdateButtonSprite();
        }
    }

    private void UpdateButtonSprite()
    {
        if (muteButton != null && muteButton.image != null && AudioEffects.Instance != null)
        {
            muteButton.image.sprite = AudioEffects.Instance.IsMuted() ? musicOffSprite : musicOnSprite;
        }
    }
}