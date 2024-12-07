using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryWall : MonoBehaviour
{

    Rigidbody2D rb;
    Collider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerController.Instance.isAttacking)
            {
                StartCoroutine(DestroyCoroutine());
            }
        }
    }

    IEnumerator DestroyCoroutine()
    {
        Vector3 dir = (transform.position - PlayerController.Instance.GetShapeTransform().position).normalized;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(dir * 10, ForceMode2D.Impulse);
        col.enabled = false;

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
