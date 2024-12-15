using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMissile : BulletBase
{
    Transform target;
    int move = 0;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Move());
    }

    void FixedUpdate()
    {
        if (move == 0)
        {
            rb.AddForce(Vector2.up, ForceMode2D.Impulse);

            return;
        }

        target = PlayerController.Instance.GetShapeTransform();

        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            rb.AddForce(direction * 2f, ForceMode2D.Impulse);
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(0.2f);
        move = 1;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.CompareTag("Bullet") == false && collision.gameObject.CompareTag("Camera") == false)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
