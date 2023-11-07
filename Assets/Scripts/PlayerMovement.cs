using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveDirection = Vector2.up;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Vector2.right;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // O jogador continuará se movendo na direção atual após a colisão.
    }
}
