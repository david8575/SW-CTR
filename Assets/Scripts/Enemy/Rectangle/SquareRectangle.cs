using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRectangle : MonoBehaviour
{
    public float speed = 2.0f;
    public float jumpHeight = 5.0f;
    public float dashSpeed = 10.0f;
    public float health = 100.0f;
    public GameObject miniSquarePrefab;
    public GameObject homingMissilePrefab;
    public Transform[] obstacles;  // 장애물 배열 이 방법이 맞는 지는 모르겠음
    public float mapWidth = 100.0f; // 맵 크기를 몰라서 임의 부여

    private Transform player;
    private bool isJumping = false;
    private float nextAttackTime = 0;
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
         // 체력 상태에 따른 행동 변화
        if (health > 80)
        {
            BasicAttack();
        }
        else if (health <= 80 && health > 50)
        {
            HalfMapAttack();
        }
        else if (health <= 50 && health > 30)
        {
            TeleportAndTransform();
        }
        else if (health <= 30)
        {
            ThreeQuartersMapAttack();
        }
    }

    private void BasicAttack()
    {
        if (Time.time >= nextAttackTime && !isJumping)
        {
            StartCoroutine(JumpAttack());
            SpawnMiniSquares();
            nextAttackTime = Time.time + 3.0f;
        }
    }

    private void SpawnMiniSquares()
    {
        // 미니 정사각형을 소환해 플레이어를 향해 돌진하게 만듬
        GameObject miniSquare = Instantiate(miniSquarePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        miniSquare.GetComponent<Rigidbody2D>().velocity = direction * dashSpeed;
    }

    private void HalfMapAttack()
    {
        // 맵의 절반 크기로 확장하여 내리찍기 공격
        transform.localScale = new Vector3(mapWidth / 2, originalScale.y, originalScale.z);
        StartCoroutine(JumpAttack());
    }

    private void TeleportAndTransform()
    {
        // 장애물 중 하나로 랜덤 텔레포트
        int randomIndex = Random.Range(0, obstacles.Length);
        Transform chosenObstacle = obstacles[randomIndex];
        transform.position = chosenObstacle.position;

        // 일정 시간 후 장애물이 유도 미사일로 변신
        StartCoroutine(TransformIntoMissile(chosenObstacle));
    }

    private IEnumerator TransformIntoMissile(Transform obstacle)
    {
        yield return new WaitForSeconds(5.0f);  // 장애물로 유지되는 시간
        Destroy(obstacle.gameObject);  // 장애물 제거
        GameObject missile = Instantiate(homingMissilePrefab, transform.position, Quaternion.identity);
    }

    private void ThreeQuartersMapAttack()
    {
        // 맵의 75% 크기로 확장하여 내리찍기 공격
        transform.localScale = new Vector3(mapWidth * 0.75f, originalScale.y, originalScale.z);
        StartCoroutine(JumpAttack());
    }

    private IEnumerator JumpAttack()
    {
        isJumping = true;

        // 위로 점프
        Vector2 jumpTarget = new Vector2(transform.position.x, transform.position.y + jumpHeight);
        while (Vector2.Distance(transform.position, jumpTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, jumpTarget, speed * Time.deltaTime);
            yield return null;
        }

        // 플레이어를 향해 내려찍기
        Vector2 attackTarget = new Vector2(player.position.x, transform.position.y);
        while (Vector2.Distance(transform.position, attackTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, attackTarget, speed * 2 * Time.deltaTime);
            yield return null;
        }

        // 공격 후 원래 크기로 복귀
        transform.localScale = originalScale;
        isJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 추후 플레이어에게 데미지를 주는 로직 추가 
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
