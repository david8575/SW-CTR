using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OblongRectangle : EnemyBase
{
    public float jumpForce = 10f;
    public float slamForce = 20f;
    private Vector3 originalScale;

    public string DashSound = "Shoot17";
    public string JumpSound = "jump_6";
    public string ScaleChangeSound = "powerup_36";

    protected override void Start()
    {
        base.Start();
        originalScale = transform.localScale;
    }

    protected override IEnumerator Attack()
    {
        if (player == null)
        {
            player = PlayerController.Instance?.GetShapeTransform();
            if (player == null)
            {
                Debug.LogWarning("Player reference is null in DynamicRectangle Attack.");
                yield break;
            }
        }

        
        if (originalScale == Vector3.zero)
        {
            Debug.LogWarning("Original scale is zero. Resetting to default values.");
            originalScale = new Vector3(1, 1, 1);
        }

        Vector2 jumpDirection = new Vector2(0, 1);
        AudioManager.PlaySound(JumpSound);
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

        AudioManager.PlaySound(ScaleChangeSound);
        transform.localScale = new Vector3(originalScale.x * 2, originalScale.y, originalScale.z);

        yield return new WaitForSeconds(0.5f);

        IsAttacking = true;
        Vector2 slamDirection = new Vector2(0, -1);
        AudioManager.PlaySound(DashSound);
        rb.AddForce(slamDirection * slamForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        transform.localScale = originalScale;
        IsAttacking = false;
    }
}
