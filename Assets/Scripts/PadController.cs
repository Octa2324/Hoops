using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadController : MonoBehaviour
{
    public float bounceMultiplier = 2f;

    private AudioEffects audioEffects;

    private void Start()
    {
        audioEffects = FindObjectOfType<AudioEffects>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRigidbody != null)
            {
                ballRigidbody.velocity = ballRigidbody.velocity * bounceMultiplier;
            }

            if (audioEffects != null)
            {
                audioEffects.Boing();
            }
        }
    }
}
