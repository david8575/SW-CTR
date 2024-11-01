using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float jumpForce;
    public float attack;
    public float defense;
    public float cooldown;
    public float specialPower;

    public Rigidbody2D rb;
    protected PlayerController controller;

    public bool canJump = true;
    public bool canSpecial = true;

    public void Init(PlayerController con)
    {
        rb = GetComponent<Rigidbody2D>();
        controller = con;
    }

    public abstract void OnSpecialStarted();

    public abstract void OnSpecialCanceled();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.transform.position.y < transform.position.y)
            {
                canJump = true;
                canSpecial = true;
            }
        }
    }
}
