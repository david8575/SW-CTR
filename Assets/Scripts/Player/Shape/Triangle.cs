using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Triangle : Shape
{
    public override void OnSpecialStarted()
    {

        // 특수 능력 누르자마자 마우스 방향으로 특수 능력 사용
        Vector2 target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        // 좌우로만 발사
        target.y = 0;

        // 좌우 속력은 잃어버림
        rb.velocity = new Vector2(0, rb.velocity.y);

        // 발사하기
        rb.AddForce(target.normalized * specialPower, ForceMode2D.Impulse);


        StartCoroutine(AttackTime(0.5f));
    }

    IEnumerator AttackTime(float time)
    {
        // time만큼만 공격모드 후 종료
        controller.isAttacking = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(time);
        spriteRenderer.color = Color.white;
        controller.isAttacking = false;
    }
}
