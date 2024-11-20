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

        // 특수능력키 누르면 차징 시작
        isCharging = true;
        power = 0.1f;
    }

    private void FixedUpdate()
    {
        // 차징 중에는 힘이 계속해서 증가함
        if (isCharging)
        {
            power *= 2;

            // 특수능력 스탯이 최대치
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

        // 마우스를 때면 마우스 방향으로 모은 힘만큼 발사
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector3 dir = (mousePos - transform.position).normalized;
        rb.AddForce(dir * power, ForceMode2D.Impulse);

    }
}
