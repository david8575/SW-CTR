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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // ���� �������� ���� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.transform.position.y < transform.position.y)
            {
                if (controller.isAttacking)
                {
                    controller.isAttacking = false;
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }
}
