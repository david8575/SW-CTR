using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircle : EnemyBase
{
    [Header("Circle Specific")]
    public GameObject[] enemyPrefabs; // 랜덤 소환할 적 프리팹
    public GameObject missilePrefab; // 미사일 프리팹
    public GameObject laserPrefab; // 레이저 프리팹
    public Transform mapCenter; // 맵 중심 위치
    private Sprite defaultSprite;
    public Sprite secondFormSprite;
    public Sprite thirdFormSprite;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
    }

    protected override IEnumerator Attack()
    {
        int randomAttack = Random.Range(1, 7); // 1~6 랜덤 선택
        Debug.Log($"CircleEnemy performing attack {randomAttack}");

        switch (randomAttack)
        {
            case 1:
                yield return SummonRandomEnemy();
                break;
            case 2:
                yield return GrowAtCenter();
                break;
            case 3:
                yield return TransformAndShoot();
                break;
            case 4:
                yield return TransformAndLaser();
                break;
            case 5:
                yield return ChargeAndRush();
                break;
            case 6:
                yield return BounceRush();
                break;
        }
    }

    private IEnumerator SummonRandomEnemy()
    {
        GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(randomEnemy, transform.position + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity);
        yield return null;
    }

    private IEnumerator GrowAtCenter()
    {
        // 맵 중심으로 이동
        while (Vector2.Distance(transform.position, mapCenter.position) > 0.5f)
        {
            Vector2 direction = (mapCenter.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 점진적으로 커지기
        Vector3 originalScale = transform.localScale;
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 3f, t);
            yield return null;
        }

        // 일정 시간 대기
        yield return new WaitForSeconds(2f);

        // 원래 크기로 돌아가기
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalScale * 3f, originalScale, t);
            yield return null;
        }
    }

    private IEnumerator TransformAndShoot()
    {
        spriteRenderer.sprite = secondFormSprite;

        // 미사일 발사
        // for (int i = 0; i < 5; i++)
        // {
        //    Instantiate(missilePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        //}

        yield return new WaitForSeconds(1f);
        spriteRenderer.sprite = defaultSprite; // 원래 모습으로 복귀
    }

    private IEnumerator TransformAndLaser()
    {
        spriteRenderer.sprite = thirdFormSprite;

        // 레이저 발사 및 회전
        // GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        // float rotationSpeed = 100f;

        // for (float t = 0; t < 3f; t += Time.deltaTime)
        // {
        //    laser.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        yield return null;
       // }

        // Destroy(laser);
        spriteRenderer.sprite = defaultSprite; // 원래 모습으로 복귀
    }

    private IEnumerator ChargeAndRush()
    {
        yield return new WaitForSeconds(1.5f); // 차징 시간
        Vector2 direction = (player.position - transform.position).normalized;
        rb.AddForce(direction * moveSpeed * 20f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
    }

    private IEnumerator BounceRush()
    {
        yield return new WaitForSeconds(0.8f); // 짧은 차징 시간
        Vector2 direction = (player.position - transform.position).normalized;
        rb.AddForce(direction * moveSpeed * 15f, ForceMode2D.Impulse);

        for (int i = 0; i < 5; i++) // 벽에 튕기는 동작
        {
            yield return new WaitForSeconds(0.5f);
            Vector2 bounceDirection = Vector2.Reflect(rb.velocity.normalized, Random.insideUnitCircle.normalized);
            rb.AddForce(bounceDirection * moveSpeed * 10f, ForceMode2D.Impulse);
        }

        rb.velocity = Vector2.zero;
    }
}
