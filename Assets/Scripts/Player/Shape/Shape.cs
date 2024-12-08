using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public PlayerStatus status;

    // 도형의 능력치
    public float speed { get { return status.Speed; } }
    public float jumpForce { get { return status.JumpPower; } }
    public float attack { get { return status.Attack; } }
    public float defense { get { return status.Defense; } }
    public float cooldown { get { return status.Cooldown; } }
    public float specialPower { get { return status.SpecialPower; } }

    public bool IsInvincible = false;

    // 이동 관련 컴포넌트
    public Rigidbody2D rb;
    protected PlayerController controller;
    protected SpriteRenderer spriteRenderer;
    protected Color color;
    protected Coroutine attackCoroutine;

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

            Vector2 dir = (enemy.transform.position - transform.position).normalized;
            //Debug.DrawRay(transform.position, dir, Color.red, 1f);

            if (defense > 0)
                rb.AddForce(1f * (enemy.attackPower / defense) * -dir, ForceMode2D.Impulse);
            else
                rb.AddForce(1f * enemy.attackPower * -dir, ForceMode2D.Impulse);

            enemy.AddForce(1f * (attack) * dir);

            if (controller.isAttacking)
            {
                enemy.TakeDamage(attack);
            }

            if (enemy.IsAttacking)
            {
                if (defense < enemy.attackPower)
                    controller.TakeDamage(enemy.attackPower);
            }

            StopAttack();

            /*
            if (controller.isAttacking && enemy.IsAttacking)
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
            else if (enemy.IsAttacking)
            {
                controller.TakeDamage(enemy.attackPower);
            }
            */
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            var contact = collision.GetContact(collision.contactCount / 2);
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

    protected virtual void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        controller.isAttacking = false;
        spriteRenderer.color = color;
    }

    public IEnumerator Invincible(float time)
    {
        IsInvincible = true;
        Color first = new Color(color.r, color.g, color.b, 0.4f);
        Color second = new Color(color.r, color.g, color.b, 0.7f);
        var wait = new WaitForSeconds(0.05f);
        bool idx = true;

        float t = 0;
        while (t < time)
        {
            spriteRenderer.color = idx ? first : second;
            idx = !idx;
            t += 0.05f;
            yield return wait;
        }

        spriteRenderer.color = color;
        IsInvincible = false;
    }
}
