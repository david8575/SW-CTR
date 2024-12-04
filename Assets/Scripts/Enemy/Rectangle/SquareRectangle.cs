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
    private Vector3 startPos;
    protected override void Start()
    {
        base.Start();
        maxHp = health;
        startPos = transform.position;
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
            Debug.Log("basicAttack");
            rb.gravityScale = 0f;

            Vector3 targetPosition = player.position;
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
            Debug.Log("summon mini");
            Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
            Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
