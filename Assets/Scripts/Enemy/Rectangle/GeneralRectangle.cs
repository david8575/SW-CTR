using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRectangle : EnemyBase
{
    public float jumpForce = 10f;
    public float slamForce = 20f;

    protected override IEnumerator Attack()
    {
        if (player == null)
        {
            player = PlayerController.Instance?.GetShapeTransform();
            if (player == null)
            {
                Debug.LogWarning("Player reference is null in NormalSquare Attack.");
                yield break;
            }
        }

        Vector2 jumpDirection = new Vector2(0, 1);
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        IsAttacking = true;
        Vector2 slamDirection = new Vector2(0, -1);
        rb.AddForce(slamDirection * slamForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
    }
}
