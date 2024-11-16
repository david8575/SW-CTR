using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    // 도형의 능력치
    public float speed;
    public float jumpForce;
    public float attack;
    public float defense;
    public float cooldown;
    public float specialPower;

    // 이동 관련 컴포넌트
    public Rigidbody2D rb;
    protected PlayerController controller;
    protected SpriteRenderer spriteRenderer;

    // 초기화
    public void Init(PlayerController con)
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = con;
    }

    // 특수 능력 추상 함수
    public abstract void OnSpecialStarted();

    public virtual void OnSpecialCanceled() { }

    // 땅 접촉시 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (controller.isAttacking)
            {
                collision.gameObject.GetComponent<AcuteTriangleEnmey>().TakeDamage();
            }
            else
            {

            }
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            var contact = collision.GetContact(0);
            if (contact.point.y < transform.position.y)
            {
                ActiveJump();
            }
        }


    }

    // 땅 위에서 접촉시 발동하는 함수
    protected virtual void ActiveJump()
    {
        controller.canJump = true;
    }
}
