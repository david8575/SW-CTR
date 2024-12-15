using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Square : Shape
{

    public string ShockWaveSound;
    public ShockWave shock;

    public override void OnSpecialStarted()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, LayerMask.GetMask("Wall"));
        if (hit.collider != null)
            return;

        AudioManager.PlaySound(SpecialSound);

        // 특수 능력 누르자마자 밑으로 강한 힘 부여
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.down * specialPower, ForceMode2D.Impulse);

        // 내려가는 중에는 공격모드
        controller.isAttacking = true;
        spriteRenderer.color = Color.red;
    }

    protected override void ActiveJump()
    {
        base.ActiveJump();

        // 공격 중이면 공격 종료
        if (controller.isAttacking)
        {
            ApeearShock();

            controller.isAttacking = false;
            spriteRenderer.color = color;

            
        }
    }

    protected override void StopAttack()
    {
        base.StopAttack();

        ApeearShock();
    }

    void ApeearShock()
    {
        shock.gameObject.SetActive(true);
        shock.Appear(transform.position);
        AudioManager.PlaySound(ShockWaveSound);
    }

    private void OnDisable()
    {
        shock.gameObject.SetActive(false);
    }
}
