using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMissile : BulletBase
{
    Transform target;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        rb.AddForce(Vector2.up * 1f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);

        target = PlayerController.Instance.GetShapeTransform();

        float time = 0;
        Vector2 direction = (target.position - transform.position).normalized;
        while (time < 1f)
        {        
            direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 135);
            time += Time.deltaTime;
            yield return null;
        }

        rb.AddForce(direction * 20f, ForceMode2D.Impulse);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Bullet") == false && collision.gameObject.CompareTag("Enemy") == false)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
