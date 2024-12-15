using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhombusRectangle : EnemyBase
{
    public float jumpHeight = 5f; 
    public float attackForce = 7f;

    public string DashSound = "Shoot17";
    public string JumpSound = "jump_6";

    protected override IEnumerator Attack()
    {
        Vector2 jumpTarget = new Vector2(player.transform.position.x, player.transform.position.y + 2f); 
        Vector2 jumpDir = (jumpTarget - (Vector2)transform.position).normalized;

        AudioManager.PlaySound(JumpSound);
        rb.velocity = new Vector2(jumpDir.x * jumpHeight, jumpHeight); 
        yield return new WaitForSeconds(0.5f); 

        Vector2 attackDir = (player.transform.position - transform.position).normalized;
        yield return new WaitForSeconds(0.3f);

        IsAttacking = true;
        AudioManager.PlaySound(DashSound);
        rb.AddForce(attackDir * attackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }
}
