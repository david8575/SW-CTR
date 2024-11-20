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

    public bool IsInvincible = false;

    // 이동 관련 컴포넌트
    public Rigidbody2D rb;
    protected PlayerController controller;
    protected SpriteRenderer spriteRenderer;
    protected Color color;

    // 초기화
    public void Init(PlayerController con)
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        controller = con;
    }

    // 특수 능력 추상 함수
    public abstract void OnSpecialStarted();

    public virtual void OnSpecialCanceled() { }

    // 접촉시
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            bool isKill = false;

            Vector2 dir = (enemy.transform.position - transform.position).normalized;
            //Debug.DrawRay(transform.position, dir, Color.red, 1f);

            if (defense > 0)
                rb.AddForce(1f * (enemy.attackPower / defense) * -dir, ForceMode2D.Impulse);
            else
                rb.AddForce(1f * enemy.attackPower * -dir, ForceMode2D.Impulse);

            enemy.AddForce(1f * (attack) * dir);

            if (controller.isAttacking && enemy.isAttacking)
            {
                if (attack >= enemy.attackPower)
                {
                    isKill = enemy.TakeDamage(attack);
                }
                else
                {
                    controller.TakeDamage(enemy.attackPower);
                }
            }
            else if (controller.isAttacking)
            {
                isKill = enemy.TakeDamage(attack);
            }
            else if (enemy.isAttacking)
            {
                controller.TakeDamage(enemy.attackPower);
            }

            if (isKill)
            {
                controller.hp += enemy.healingAmount;
                if (controller.hp > controller.maxHp)
                    controller.hp = controller.maxHp;
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

    public IEnumerator Invincible(float time)
    {
        IsInvincible = true;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0.5f);

        yield return new WaitForSeconds(time);

        spriteRenderer.color = color;
        IsInvincible = false;
    }
}
