using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Circle : Shape
{
    bool isCharging = false;
    float power;
    
    public override void OnSpecialStarted()
    {

        // Ư���ɷ�Ű ������ ��¡ ����
        isCharging = true;
        power = 0.1f;
    }

    private void FixedUpdate()
    {
        // ��¡ �߿��� ���� ����ؼ� ������
        if (isCharging)
        {
            if (power < specialPower)
            {         
                power *= 2;
                float amount = power / specialPower;
                spriteRenderer.color = new Color(1f - amount, 1f - amount, 1f - amount, 1);
            }

            // Ư���ɷ� ������ �ִ�ġ
            if (power > specialPower)
            {
                power = specialPower;
                spriteRenderer.color = Color.red;
            }
        }
    }

    public override void OnSpecialCanceled()
    {
        if (controller.canSpecial == false || isCharging == false)
            return;
        isCharging = false;
        spriteRenderer.color = Color.white;

        //rb.velocity = Vector2.zero;

        // ���콺�� ���� ���콺 �������� ���� ����ŭ �߻�
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector3 dir = (mousePos - transform.position).normalized;
        float dot = Vector3.Dot(dir, rb.velocity);
        if (dot < 0)
            rb.velocity = Vector2.zero;
        rb.AddForce(dir * power, ForceMode2D.Impulse);

    }
}
