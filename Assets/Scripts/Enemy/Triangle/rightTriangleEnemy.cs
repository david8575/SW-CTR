using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightTriangleEnemy : EnemyBase
{
    public float dashSpeed = 15f;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    protected override IEnumerator Attack()
    {
        
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        IsAttacking = true; 

        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
    }
}
