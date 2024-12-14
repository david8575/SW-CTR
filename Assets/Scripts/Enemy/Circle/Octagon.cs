using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octagon : EnemyBase
{
    
    [Header("Octagon Specific")]
    public Sprite normalSprite; // 기본 스프라이트
    public Sprite poweredUpSprite; // 체력 50% 이하 시 스프라이트
    private SpriteRenderer spriteRenderer;

    public string DashSound = "Shoot17";
    public string PowerUpSound = "MotionTracker_Scan1";

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // 초기 스프라이트 설정
    }

    protected override IEnumerator Attack()
    {
        // 돌진 공격
        Vector2 direction = (player.position - transform.position).normalized;

        AudioManager.PlaySound(DashSound);
        rb.AddForce(direction * moveSpeed * 10f, ForceMode2D.Impulse);

        // 쿨타임 대기
        yield return new WaitForSeconds(attackCoolDown);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // 체력이 50% 이하일 때 방어력 증가 및 스프라이트 변경
        if (health <= status.Health * 0.5f && spriteRenderer.sprite != poweredUpSprite)
        {
            AudioManager.PlaySound(PowerUpSound);
            defense *= 2; // 방어력 2배 증가
            spriteRenderer.sprite = poweredUpSprite; // 스프라이트 변경
            Debug.Log("OctagonEnemy powered up");
        }
    }
}

