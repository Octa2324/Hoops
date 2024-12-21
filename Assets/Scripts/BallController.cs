using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D col;
    [HideInInspector] public Vector3 pos { get { return transform.position; } }

    private LevelManager levelManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (!levelManager.HasScored())
            {
                levelManager.ShowGameOverCanvas();
            }
        }
    }
}
