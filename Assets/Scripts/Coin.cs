using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private LevelManager levelManager;  

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) 
        {
            if (levelManager != null)
            {
                levelManager.CollectCoin();
            }
            Destroy(gameObject);
        }
    }
}
