using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TMP_Text starCountText; 
    private int totalStars; 

    void Start()
    {
        totalStars = PlayerPrefs.GetInt("TotalStars", 0);
        UpdateUI();
    }

    private void UpdateUI()
    {
        starCountText.text = "" + totalStars;
    }
}
