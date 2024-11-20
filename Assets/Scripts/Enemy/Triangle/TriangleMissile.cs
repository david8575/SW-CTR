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
            rb.AddForce(Vector2.up * 3f);

            return;
        }

        target = PlayerController.Instance.GetShapeTransform();

        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            rb.AddForce(direction * 7f);
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(1f);
        move = 1;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Bullet") == false)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
}
