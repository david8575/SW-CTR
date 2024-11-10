using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRectangle : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 5.0f;
    public float jumpForce = 7.0f;  // 점프의 힘
    public float jumpAttackCooldown = 3.0f;

    private Rigidbody2D rb;
    private Transform player;
    private bool isAlive = true;
    private bool isJumping = false;
    private float nextJumpTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
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
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        if (transform.position.x > 3.0f || transform.position.x < -3.0f)
        {
            speed = -speed; // 방향 전환
        }
    }

    private IEnumerator JumpAttack()
    {
        isJumping = true;

        // 위로 점프 (Rigidbody를 사용하여 점프)
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        yield return new WaitForSeconds(0.5f);  // 잠시 대기하여 점프 시간 확보

        // 주인공 방향으로 떨어지기
        Vector2 attackTarget = new Vector2(player.position.x, transform.position.y - 1.0f);  // 목표 위치 설정
        while (transform.position.y > attackTarget.y)
        {
            rb.velocity = new Vector2((player.position.x - transform.position.x) * 2.0f, rb.velocity.y);  // 목표를 향해 이동
            yield return null;
        }

        rb.velocity = Vector2.zero;  // 착지 후 속도 초기화
        isJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 주인공과의 상호작용 (추후 체력 감소 등의 로직 추가 가능)
            Debug.Log("SquareEnemy collided with Player!");
        }
    }
}
