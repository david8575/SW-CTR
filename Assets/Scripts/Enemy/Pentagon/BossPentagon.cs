using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPentagon : MonoBehaviour
{
    public float speed = 2.0f;
    public float dashSpeed = 10.0f;
    public float health = 100.0f;
    public GameObject miniPentagonPrefab;
    public GameObject missilePrefab;
    public GameObject trianglePrefab;
    public GameObject squarePrefab;
    public float mapWidth = 20.0f;
    public float chargeDuration = 3.0f;

    private Transform player;
    private bool isCharging = false;
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
        // 체력 상태에 따른 공격 패턴
        if (health > 80)
        {
            BasicAttack();
        }
        else if (health <= 80 && health > 50)
        {
            StartCoroutine(StrongBounceDash());
        }
        else if (health <= 50 && health > 30)
        {
            StarMissileAttack();
        }
        else if (health <= 30)
        {
            SpawnShapes();
        }
    }

    private void BasicAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(DashAttack());
            SpawnMiniPentagons();
            nextAttackTime = Time.time + 3.0f;
        }
    }

    private void SpawnMiniPentagons()
    {
        // 소형 정오각형 소환 후 플레이어를 향해 돌진
        GameObject miniPentagon = Instantiate(miniPentagonPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        miniPentagon.GetComponent<Rigidbody2D>().velocity = direction * dashSpeed;
    }

    private IEnumerator DashAttack()
    {
        // 주인공 방향으로 돌진 공격
        Vector2 dashDirection = (player.position - transform.position).normalized;
        float dashDuration = 0.5f;
        float dashStartTime = Time.time;

        while (Time.time < dashStartTime + dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator StrongBounceDash()
    {
        // 오랜 시간 차징 후 강한 돌진 공격 (벽에 튕김)
        if (isCharging) yield break;
        isCharging = true;

        yield return new WaitForSeconds(chargeDuration);

        Vector2 direction = (player.position - transform.position).normalized;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * dashSpeed * 2;  // 강한 돌진

        yield return new WaitForSeconds(1.0f);
        rb.velocity = Vector2.zero;  // 튕김 효과 후 속도 초기화
        isCharging = false;
    }

    private void StarMissileAttack()
    {
        // 5개의 꼭짓점에서 미사일 발사
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnPosition = transform.position + Quaternion.Euler(0, 0, i * 72) * Vector3.up * 1.5f;
            GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
           
        }
    }

    private void SpawnShapes()
    {
        // 내부에 선이 그어지고 강력한 삼각형과 사각형 소환
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 주는 로직 추가
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
