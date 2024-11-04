using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Triangle : Shape
{
    public override void OnSpecialStarted()
    {
        if (controller.canSpecial == false)
            return;

        Vector2 target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        target.y = 0;

        rb.velocity = new Vector2(0, rb.velocity.y);

        rb.AddForce(target.normalized * specialPower, ForceMode2D.Impulse);


        StartCoroutine(AttackTime(0.5f));
    }

    public override void OnSpecialCanceled()
    {
        
    }

    IEnumerator AttackTime(float time)
    {
        controller.isAttacking = true;
        yield return new WaitForSeconds(time);
        controller.isAttacking = false;
    }
}
