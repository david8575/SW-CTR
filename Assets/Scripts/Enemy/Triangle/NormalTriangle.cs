using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTriangle : EnemyBase
{
    public float attackForce = 5f;

    protected override IEnumerator Attack()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        isAttacking = true;
        rb.AddForce(dir * attackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }
}
