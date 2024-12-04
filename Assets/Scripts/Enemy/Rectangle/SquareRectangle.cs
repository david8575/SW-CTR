using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRectangle : EnemyBase
{
     public bool startFight = false;

    [Header("Square Boss Settings")]
    [SerializeField]
    int step = 2; 
    float smashHeight = 15f; 
    float attackForce = 20f;
    float maxHp;

    [SerializeField]
    Transform miniSquareSpawnPoint; 

    [Header("Prefabs")]
    public GameObject miniSquarePrefab;
    public GameObject enclosingWallPrefab;
    private Vector3 startPos;
    private float mapWidth;
    protected override void Start()
    {
        base.Start();
        maxHp = health;
        startPos = transform.position;
        mapWidth = 100f;
    }

    protected override void ApproachPlayer()
    {
        //base.ApproachPlayer();
    }

    protected override IEnumerator Attack()
    {
        if(!startFight)
        {
            yield return new WaitForSeconds(1f);
            startFight = true;
        }

        yield return new WaitForSeconds(0.5f);

        int rnd = UnityEngine.Random.Range(0, step);

        if (rnd == 0)
        {
            Debug.Log("대기 중");
            yield return new WaitForSeconds(1f);
        }

        else if (rnd == 1)
        {
            Debug.Log("BossSquare: Basic Downward Smash");
            rb.gravityScale = 0f;

             Vector3 targetPosition = player.position;
            targetPosition.x += UnityEngine.Random.Range(-3f, 3f); 
            targetPosition.y += smashHeight; 
            transform.position = targetPosition;

            yield return new WaitForSeconds(0.5f);

            rb.gravityScale = 1f;
            IsAttacking = true;
            rb.AddForce(Vector2.down * attackForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.5f);

            IsAttacking = false;
        }

        else if (rnd == 2)
        {
            Debug.Log("꼬맹이들 소환");
            Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
            Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        else if (rnd == 3)
        {
            Debug.Log("50% 크고 내려찍기");
            yield return StartCoroutine(PerformDownwardSmash(mapWidth / 2f));
        }
        else if (rnd == 4)
        {
            Debug.Log("순간이동");
            yield return StartCoroutine(TeleportToFloatingPlatform());
        }
        else if (rnd == 5)
        {
            Debug.Log("75% 크고 내려찍기");
            yield return StartCoroutine(PerformDownwardSmash(mapWidth * 0.75f));
            yield return StartCoroutine(EnclosePlayer());
        }

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;

        float time = 0f;
        while (Vector2.Distance(transform.position, startPos) > 0.3f)
        {
            transform.position = Vector2.Lerp(transform.position, startPos, 0.1f);
            yield return new WaitForSeconds(0.05f);

            time += 0.05f;
            if (time > 2.5f)
            {
                break;
            }
        }

        transform.position = startPos;
    }

    IEnumerator PerformDownwardSmash(float width)
    {
        rb.gravityScale = 0f;

        Vector3 targetPosition = player.position;
        targetPosition.x += UnityEngine.Random.Range(-3f, 3f); 
        targetPosition.y += smashHeight;
        transform.position = targetPosition;

        Vector3 originalScale = transform.localScale;
        transform.localScale = new Vector3(width, originalScale.y, originalScale.z);

        yield return new WaitForSeconds(0.5f);

        rb.gravityScale = 1f;
        IsAttacking = true;
        rb.AddForce(Vector2.down * attackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);

        transform.localScale = originalScale;
        IsAttacking = false;
    }

    IEnumerator TeleportToFloatingPlatform()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag(tag);

        if (platforms.Length == 0)
        {
            Debug.LogWarning($"태그 '{tag}'를 가진 지형물이 없습니다!");
            yield break;
        }

        GameObject chosenPlatform = platforms[UnityEngine.Random.Range(0, platforms.Length)];
        transform.position = chosenPlatform.transform.position;

        yield return new WaitForSeconds(1f);
    }

    IEnumerator EnclosePlayer()
    {
        Vector3 playerPosition = player.position;

        GameObject leftWall = Instantiate(enclosingWallPrefab, playerPosition + Vector3.left * 2f, Quaternion.identity);
        GameObject rightWall = Instantiate(enclosingWallPrefab, playerPosition + Vector3.right * 2f, Quaternion.identity);
        GameObject topWall = Instantiate(enclosingWallPrefab, playerPosition + Vector3.up * 2f, Quaternion.identity);
        GameObject bottomWall = Instantiate(enclosingWallPrefab, playerPosition + Vector3.down * 2f, Quaternion.identity);

        yield return new WaitForSeconds(3f);

        Destroy(leftWall);
        Destroy(rightWall);
        Destroy(topWall);
        Destroy(bottomWall);
    }

    public override bool TakeDamage(float damage)
    {
        bool isDead = base.TakeDamage(damage);

        if (isDead)
        {
            return true;
        }

        if (health <= maxHp * 0.8f && step < 3)
        {
            Debug.Log("보스 피 80");
            step = 3;
        }

         else if (health <= maxHp * 0.5f && step < 4)
        {
            Debug.Log("보스 피 50");
            attackCoolDown -= 0.5f; 
            step = 4;
        }
        else if (health <= maxHp * 0.3f && step < 5)
        {
            Debug.Log("보스 피 30");
            Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
            step = 5;
        }

        return false;
    }

    protected override void Die()
    {
        Debug.Log("주금");
        base.Die();
    }
}
