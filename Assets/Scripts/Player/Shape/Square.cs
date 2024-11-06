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

        // Ư�� �ɷ� �����ڸ��� ������ ���� �� �ο�
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.down * specialPower, ForceMode2D.Impulse);

        // �������� �߿��� ���ݸ��
        controller.isAttacking = true;
        spriteRenderer.color = Color.red;
    }

    protected override void ActiveJump()
    {
        base.ActiveJump();

        // ���� ���̸� ���� ����
        if (controller.isAttacking)
        {
            controller.isAttacking = false;
            spriteRenderer.color = Color.black;
        }
    }
}
