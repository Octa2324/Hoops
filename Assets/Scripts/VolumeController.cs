using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private Button muteButton;  
    [SerializeField] private Image muteButtonImage; 
    [SerializeField] private Sprite unmutedSprite; 
    [SerializeField] private Sprite mutedSprite;   


    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioManager.Instance.Volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (muteButton != null)
        {
            muteButton.onClick.AddListener(ToggleMute);
        }

        UpdateMuteButtonImage();
    }

    private void SetVolume(float value)
    {
        AudioManager.Instance.Volume = value;
        UpdateMuteButtonImage();  
    }

    private void ToggleMute()
    {
        AudioManager.Instance.ToggleMute();
        volumeSlider.value = AudioManager.Instance.Volume;  
        UpdateMuteButtonImage();  
    }

    private void UpdateMuteButtonImage()
    {
        if (muteButtonImage != null)
        {
            muteButtonImage.sprite = AudioManager.Instance.IsMuted() ? mutedSprite : unmutedSprite;
        }
    }
}
