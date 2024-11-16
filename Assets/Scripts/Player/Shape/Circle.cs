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
            power *= 2;

            // Ư���ɷ� ������ �ִ�ġ
            if (power > specialPower)
            {
                power = specialPower;
            }
        }
    }

    public override void OnSpecialCanceled()
    {
        if (controller.canSpecial == false)
            return;
        isCharging = false;

        //rb.velocity = Vector2.zero;

        // ���콺�� ���� ���콺 �������� ���� ����ŭ �߻�
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector3 dir = (mousePos - transform.position).normalized;
        rb.AddForce(dir * power, ForceMode2D.Impulse);

    }
}
