using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Scoring : MonoBehaviour
{
    public bool hasScored = false;

    public event Action OnScored;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            hasScored = true;

            OnScored?.Invoke();
        }
    }
}
