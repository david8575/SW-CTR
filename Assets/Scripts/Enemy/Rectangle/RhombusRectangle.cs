using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhombusRectangle : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 5.0f;
    public float dashSpeed = 10.0f;
    public float dashCooldown = 3.0f;

    private Transform player;
    private bool isAlive = true;
    private bool isDashing = false;
    private float nextDashTime = 0;
    private float patrolTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어가 탐지 범위 내에 있을 때 돌진 공격
        if (distanceToPlayer <= detectionRange && Time.time >= nextDashTime)
        {
            if (!isDashing)
            {
                StartCoroutine(DashAttack());
                nextDashTime = Time.time + dashCooldown;
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // 좌우로 순찰
        patrolTime += Time.fixedDeltaTime;
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        
        if (patrolTime > 3f)
        {
            speed = -speed;
            patrolTime = 0;
        }
    }

    private IEnumerator DashAttack()
    {
        isDashing = true;

        // 플레이어를 향해 돌진
        Vector2 dashDirection = (player.position - transform.position).normalized;
        float dashDuration = 0.5f; // 돌진 지속 시간

        float dashStartTime = Time.time;
        while (Time.time < dashStartTime + dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 추후 주인공에게 데미지를 주는 코드 추가
        }
    }
}
