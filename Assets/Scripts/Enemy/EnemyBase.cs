using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float health = 100f;
    public float attackPower = 10f;
    public float jumpPower = 5f;
    public float moveSpeed = 2f;
    public float defense = 5f;
    public float attackCoolDown = 1.5f;
    public float detectionRange = 8f;
    public float attackRange = 4f;
    public bool isAttacking = false;

    protected float patrolInterval = 3f;
    protected float patrolTimer = 0f;
    protected int moveDirection = 1;
    protected bool canAttack = true;

    protected Transform player;
    [SerializeField]
    protected float distance;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player = PlayerController.Instance.GetShapeTransform();

        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= detectionRange)
        {
            ApproachPlayer();

            if (distance <= attackRange && canAttack)
            {
                StartCoroutine(AttackDecorator());
            }
        }
        else
        {
            Patrol();
        }
        
    }

    protected virtual void ApproachPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    protected void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolInterval)
        {
            moveDirection *= -1;
            patrolTimer = 0f;
        }

        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);
    }

    protected abstract IEnumerator Attack();

    IEnumerator AttackDecorator()
    {
        canAttack = false;
        yield return StartCoroutine(Attack());
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("공격 " + damage + " 데미지");

        if (damage > defense)
            health -= (damage - defense);

        if (health <= 0)
        {
            Die();
        }
    }

    public void AddForce(Vector2 force)
    {
        if (defense > 0)
            rb.AddForce(force / defense, ForceMode2D.Impulse);
        else
            rb.AddForce(force, ForceMode2D.Impulse);
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
