using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health = 100f;
    public float attackPower = 10f;
    public float jumpPower = 5f;
    public float moveSpeed = 2f;
    public float defense = 5f;
    public float attackCoolDown = 1.5f;
    public float detectionRange = 5f;

    protected bool isPlayerInRange = false;
    protected float attackTimer = 0f;
    protected float patrolTimer = 0f;
    protected float patrolInterval = 3f;
    protected int moveDirection = 1;
    protected GameObject player;
    protected bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
           if (health <= 0)
           {
                Die();
           }

           else
           {
                if (isPlayerInRange)
                {
                    Attack();
                }

                else
                {
                    Patrol();
                }
           }
    }

    protected void DetectPlayer()
    {
        if (player != null)
        {
            // 이방식이 맞는 지 잘 모르겠습니다
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            isPlayerInRange = distanceToPlayer <= detectionRange;
        }
    }

    protected void ApproachPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    protected void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if(patrolTimer >= patrolInterval)
        {
            moveDirection *= -1;
            patrolTimer = 0f;
        }

        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);
    }

    protected virtual IEnumerator Attack()
    {
        canAttack = false;
        
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}

