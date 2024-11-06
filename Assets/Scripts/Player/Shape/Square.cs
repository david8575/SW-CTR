using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Square : Shape
{
    public override void OnSpecialStarted()
    {
        if (controller.canSpecial == false)
            return;

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
            controller.isAttacking = false;
            spriteRenderer.color = Color.black;
        }
    }
}
