using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OblongRectangle : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 5.0f;
    public float jumpHeight = 5.0f;
    public float jumpAttackCooldown = 3.0f;

    private Transform player;
    private bool isAlive = true;
    private bool isJumping = false;
    private float nextJumpTime = 0;
    private float patrolTime = 0;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어가 탐지 범위 내에 있을 때 점프 공격
        if (distanceToPlayer <= detectionRange && Time.time >= nextJumpTime)
        {
            if (!isJumping)
            {
                StartCoroutine(JumpAttack());
                nextJumpTime = Time.time + jumpAttackCooldown;
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

    private IEnumerator JumpAttack()
    {
        isJumping = true;

        // 공격 시 좌우로 2배 확장
        transform.localScale = new Vector3(originalScale.x * 2, originalScale.y, originalScale.z);

        // 위로 점프
        Vector2 jumpTarget = new Vector2(transform.position.x, transform.position.y + jumpHeight);
        while (Vector2.Distance(transform.position, jumpTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, jumpTarget, speed * Time.deltaTime);
            yield return null;
        }

        // 주인공 위치로 내려찍기
        Vector2 attackTarget = new Vector2(player.position.x, transform.position.y);
        while (Vector2.Distance(transform.position, attackTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, attackTarget, speed * 2 * Time.deltaTime);
            yield return null;
        }

        // 원래 크기로 복귀
        transform.localScale = originalScale;
        isJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 추후 주인공에게 데미지를 주는 코드 추가 가능
        }
    }
}
