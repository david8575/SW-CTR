using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Square : Shape
{

    public ShockWave shock;

    public override void OnSpecialStarted()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, LayerMask.GetMask("Wall"));
        if (hit.collider != null)
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
            shock.gameObject.SetActive(true);
            shock.Appear(transform.position);

            controller.isAttacking = false;
            spriteRenderer.color = color;
        }
    }

    protected override void StopAttack()
    {
        base.StopAttack();

        shock.gameObject.SetActive(true);
        shock.Appear(transform.position);
    }

    private void OnDisable()
    {
        shock.gameObject.SetActive(false);
    }
}
