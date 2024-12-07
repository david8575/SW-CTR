using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPoint : MonoBehaviour
{
    public Transform respawnPoint;
    public float damage = 1f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.rb.velocity = Vector2.zero;
            PlayerController.Instance.rb.angularVelocity = 0;
            PlayerController.Instance.rb.totalForce = Vector2.zero;

            PlayerController.Instance.TakeDamage(damage);
            PlayerController.Instance.GetShapeTransform().position = respawnPoint.position;


        }
    }
}
