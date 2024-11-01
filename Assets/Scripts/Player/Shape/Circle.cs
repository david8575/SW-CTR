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
        if (canSpecial == false)
            return;

        isCharging = true;
        controller.canMove = false;

        rb.gravityScale = 0;
        power = 0.1f;
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            power *= 2;
            rb.velocity = Vector2.zero;

            if (power > specialPower)
            {
                power = specialPower;
            }
        }
    }

    public override void OnSpecialCanceled()
    {
        if (canSpecial == false)
            return;
        isCharging = false;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        rb.AddForce(dir * power, ForceMode2D.Impulse);
        rb.gravityScale = 1;

        canSpecial = false;
        controller.canMove = true;
    }
}
