using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RimController : MonoBehaviour
{
    public float upperLimit = 2f;   
    public float lowerLimit = -4f; 
    public float speed = 2f;       

    private bool movingUp = true;  

    void Update()
    {
        MoveUpDown();
    }

    private void MoveUpDown()
    {
        if (movingUp)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;

            if (transform.position.y >= upperLimit)
            {
                movingUp = false; 
            }
        }
        else
        {
            transform.position += Vector3.down * speed * Time.deltaTime;

            if (transform.position.y <= lowerLimit)
            {
                movingUp = true; 
            }
        }
    }
}
