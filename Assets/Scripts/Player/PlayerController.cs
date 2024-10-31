using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    Rigidbody2D rb;

    public float speed = 10;
    public float maxSpeed = 100;
    public float jumpPower = 10;
    float moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerInputActions = new();
        playerInputActions.Enable();

        playerInputActions.PlayerActions.Move.started += (x) => moveInput = x.ReadValue<float>();
        playerInputActions.PlayerActions.Move.canceled += (x) => moveInput = x.ReadValue<float>();

        playerInputActions.PlayerActions.Jump.performed += OnJump;
    }

    private void FixedUpdate()
    {
        if (moveInput != 0)
        {
            rb.AddForce(new Vector2(moveInput * speed, 0));
        }

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (rb.velocity.y == 0)
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
    }
}
