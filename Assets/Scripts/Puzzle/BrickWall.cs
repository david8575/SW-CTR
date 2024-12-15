using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField]
    private float destructionDelay = 3f; // Time before wall is destroyed
    [SerializeField]
    private float knockbackForce = 10f; // Force applied to the wall when hit
    [SerializeField]
    private float speedBoostMultiplier = 1.5f; // Multiplier to increase player's speed

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb == null || col == null)
        {
            Debug.LogError("Rigidbody2D or Collider2D is missing on the wall object.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerController.Instance != null)
        {
            if (PlayerController.Instance.isAttacking)
            {
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 savedVelocity = playerRb.velocity; // Save player's current velocity
                    Vector2 boostedVelocity = savedVelocity * speedBoostMultiplier; // Increase player's speed

                    // Apply boosted velocity
                    playerRb.velocity = boostedVelocity;

                    // Start wall destruction process
                    StartCoroutine(DestroyCoroutine(boostedVelocity, playerRb));
                }
            }
            else
            {
                PlayerController.Instance.canJump = true;
            }
        }
    }

    private IEnumerator DestroyCoroutine(Vector2 boostedVelocity, Rigidbody2D playerRb)
    {
        if (PlayerController.Instance == null)
        {
            yield break; // Prevent execution if player controller is null
        }

        // Apply knockback force to the wall
        Vector3 direction = (transform.position - PlayerController.Instance.GetShapeTransform().position).normalized;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Disable the collider to prevent further interactions
        col.enabled = false;

        // Wait for the specified delay before destroying the wall
        yield return new WaitForSeconds(destructionDelay);

        // Destroy the wall object
        Destroy(gameObject);
    }
}