using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenaralPentagon : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 7.0f;
    public float chargeDuration = 1.0f;
    public float dashSpeed = 15.0f;
    public float attackCooldown = 3.0f;

    private Transform player;
    private bool isCharging = false;
    private bool isDashing = false;
    private float nextAttackTime = 0;
    private Vector2 dashDirection;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if (isCharging || isDashing)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && Time.time >= nextAttackTime)
        {
            StartCoroutine(ChargeAndDash());
            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            Patrol();
        } 
    }

    private void Patrol()
    {
        // 좌우로 순찰
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        if (transform.position.x > 3.0f || transform.position.x < -3.0f)
        {
            speed = -speed; // 방향 전환
        }
    }

    private IEnumerator ChargeAndDash()
    {
        isCharging = true;

        // 차징 상태 (차징 동안 일정 시간 대기)
        dashDirection = (player.position - transform.position).normalized;
        yield return new WaitForSeconds(chargeDuration);

        // 차징 후 돌진
        isDashing = true;
        float dashTime = 0.5f;  // 돌진 지속 시간
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isCharging = false;
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 주는 로직을 추가
        }
    }
}
