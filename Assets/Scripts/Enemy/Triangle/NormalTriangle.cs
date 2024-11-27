using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTriangle : EnemyBase
{
    public float attackForce = 7f;

    protected override IEnumerator Attack()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        yield return new WaitForSeconds(0.3f);

        IsAttacking = true;
        rb.AddForce(dir * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
    }
}
