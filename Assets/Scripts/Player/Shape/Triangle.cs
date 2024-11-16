using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Triangle : Shape
{
    public override void OnSpecialStarted()
    {

        // Ư�� �ɷ� �����ڸ��� ���콺 �������� Ư�� �ɷ� ���
        Vector2 target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        // �¿�θ� �߻�
        target.y = 0;

        // �¿� �ӷ��� �Ҿ����
        rb.velocity = new Vector2(0, rb.velocity.y);

        // �߻��ϱ�
        rb.AddForce(target.normalized * specialPower, ForceMode2D.Impulse);


        StartCoroutine(AttackTime(0.5f));
    }

    IEnumerator AttackTime(float time)
    {
        // time��ŭ�� ���ݸ�� �� ����
        controller.isAttacking = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(time);
        spriteRenderer.color = Color.white;
        controller.isAttacking = false;
    }
}
