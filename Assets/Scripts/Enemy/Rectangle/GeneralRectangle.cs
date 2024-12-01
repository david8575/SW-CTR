using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRectangle : EnemyBase
{
    public float attackForce = 7f;
    public float jumpHeight = 15f;
    protected override IEnumerator Attack()
    {
        Vector2 jumpDirection = new Vector2(player.transform.position.x, player.transform.position.y + jumpHeight) - (Vector2)transform.position;
        jumpDirection = jumpDirection.normalized; 

        yield return new WaitForSeconds(0.3f);

        IsAttacking = true;
        rb.AddForce(jumpDirection * attackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        Vector2 smashDirection = Vector2.down; 
        rb.AddForce(smashDirection * attackForce * 2, ForceMode2D.Impulse); 

        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }
}
