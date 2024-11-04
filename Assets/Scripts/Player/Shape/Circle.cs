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
        if (controller.canSpecial == false)
            return;

        isCharging = true;
        power = 0.1f;
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            power *= 2;

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

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        rb.AddForce(dir * power, ForceMode2D.Impulse);

    }
}
