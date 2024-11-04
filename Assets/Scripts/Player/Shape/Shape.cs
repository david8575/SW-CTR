using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    // 도형의 능력치
    public float speed;
    public float maxSpeed;
    public float jumpForce;
    public float attack;
    public float defense;
    public float cooldown;
    public float specialPower;

    // 이동 관련 컴포넌트
    public Rigidbody2D rb;
    protected PlayerController controller;

    // 초기화
    public void Init(PlayerController con)
    {
        rb = GetComponent<Rigidbody2D>();
        controller = con;
    }

    // 특수 능력 추상 함수
    public abstract void OnSpecialStarted();

    public abstract void OnSpecialCanceled();

    // 땅 접촉시 점프 가능
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.transform.position.y < transform.position.y)
            {
                controller.canJump = true;
            }
        }
    }
}
