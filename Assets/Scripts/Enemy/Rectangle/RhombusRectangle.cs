using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhombusRectangle : EnemyBase
{
    public float jumpHeight = 5f; 
    public float attackForce = 7f; 

    protected override IEnumerator Attack()
    {
        Vector2 jumpTarget = new Vector2(player.transform.position.x, player.transform.position.y + 2f); // 머리 위로 2 유닛
        Vector2 jumpDir = (jumpTarget - (Vector2)transform.position).normalized;

        rb.velocity = new Vector2(jumpDir.x * jumpHeight, jumpHeight); 
        yield return new WaitForSeconds(0.5f); 

        Vector2 attackDir = (player.transform.position - transform.position).normalized;
        yield return new WaitForSeconds(0.3f);

        IsAttacking = true;
        rb.AddForce(attackDir * attackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }
}
