using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public EnemyStatus status;
    public float health = 100f;
    public float attackPower = 10f;
    public float jumpPower = 5f;
    public float moveSpeed = 2f;
    public float defense = 5f;
    public float attackCoolDown = 1.5f;
    public float detectionRange = 9f;
    public float attackRange = 5f;
    bool isAttacking = false;

    public bool isDead = false;

    public bool EnemyEnabled = true;
    public bool IsAttacking {
        get { return isAttacking; }
        set
        {
            isAttacking = value;
            if (attackImage != null)
                attackImage.SetActive(isAttacking);
        }
    }

    public float healingAmount;

    protected float patrolInterval = 3f;
    protected float patrolTimer = 0f;
    protected int moveDirection = 1;
    protected bool canAttack = true;

    protected Transform player;
    [SerializeField]
    protected float distance;

    protected Rigidbody2D rb;

    [Header("Other")]
    public HpBar hpBar;
    public GameObject attackImage;

    protected virtual void Start()
    {
        SetStat();

        rb = GetComponent<Rigidbody2D>();
        hpBar?.SetHp(health, health);
        if (attackImage != null)
            attackImage.SetActive(false);


        healingAmount = health * 0.1f;
        if (healingAmount < 10)
            healingAmount = 10;

        GameManager.Instance.EnemieCount++;
    }

    void SetStat()
    {
        health = status.Health;
        attackPower = status.AttackPower;
        jumpPower = status.JumpPower;
        moveSpeed = status.MoveSpeed;
        defense = status.Defense;
        attackCoolDown = status.AttackCoolDown;
        detectionRange = status.DetectionRange;
        attackRange = status.AttackRange;
        healingAmount = status.HealingAmount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!EnemyEnabled)
            return;

        player = PlayerController.Instance.GetShapeTransform();

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (isDead)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        }
        else if (distance <= detectionRange)
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
        rb.AddForce(direction * moveSpeed * 3f);
    }

    protected void Patrol()
    {
        Vector2 hitPos = new Vector2(transform.position.x + moveDirection * 0.2f, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(hitPos, Vector2.down, 1.5f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(hitPos, Vector2.down, Color.red);
        if (!hit)
        {
            moveDirection *= -1;
            patrolTimer = 0;
        }

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolInterval)
        {
            moveDirection *= -1;
            patrolTimer = 0f;
        }

        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);
    }

    protected abstract IEnumerator Attack();

    public IEnumerator AttackDecorator()
    {
        canAttack = false;
        yield return StartCoroutine(Attack());
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("enemy get " + damage);


        if (damage > defense)
        {
            health -= (damage - defense);
            AudioManager.PlaySound(status.HitSound);
        }

        hpBar?.SetHp(health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    protected virtual void Die()
    {
        if (GameManager.Instance != null) // GameManager�� �����ϴ��� Ȯ��
        {
            GameManager.Instance.CheckAllEnemiesDefeated();
        }
        else
        {
            Debug.LogWarning("GameManager instance not found!");
        }

        AudioManager.PlaySound(status.DieSound);

        StopAllCoroutines();
        StartCoroutine(DeadCoroutine());
    }

    protected IEnumerator DeadCoroutine()
    {
        
        gameObject.layer = LayerMask.NameToLayer("IgnoreAll");
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        moveSpeed *= 2f;
        isDead = true;

        yield return new WaitWhile(() => distance > 1f);

        PlayerController.Instance.Heal(healingAmount);

        Dead();
    }

    protected virtual void Dead()
    {
        Destroy(gameObject);
    }
}
