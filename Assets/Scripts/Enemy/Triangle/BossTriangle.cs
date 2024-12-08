using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossTriangle : EnemyBase
{

    // Wait, Attack, Summon triangle
    // 80% hp: Shokwave Attack
    // 50% hp: Laser Attack
    // 30% hp: Summon Right Triangle
    bool summonRightTriangle = false;

    public bool startFight = false;
    public int layer;
    public event Action DeadEvent = null;

    public int step = 3;
    float attackForce = 12f;
    float maxHp;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Vector3 startPos;

    public PhysicsMaterial2D bounceMaterial;

    [Header("SpawnPoints")]
    public Transform normalSpawnPoint;

    [Header("Prefabs")]
    public GameObject normalTriangle;
    public ShockWave shockWave;
    public GameObject targetLaser;
    public GameObject rightTriangle;

    [Header("Laser")]
    public Laser[] lasers;
    public Transform laserParent;

    protected override void Start()
    {
        base.Start();
        maxHp = health;

        startPos = transform.position;

        laserParent.gameObject.SetActive(false);

        spriteRenderer = GetComponent<SpriteRenderer>();

        detectionRange = 1000f;
        attackRange = 1000f;
        layer = gameObject.layer;
    }


    protected override void ApproachPlayer()
    {
        //base.ApproachPlayer();
    }


    protected override IEnumerator Attack()
    {
        if (startFight == false)
        {
            yield return new WaitForSeconds(1f);
            startFight = true;
        }

        yield return new WaitForSeconds(0.5f);

        int rnd = UnityEngine.Random.Range(0, step);
        float time;

        if (rnd == 0)
        {
            // wait
            yield return new WaitForSeconds(0.2f);
            Debug.Log("Wait");
        }
        else if (rnd == 1)
        {
            // summon
            Debug.Log("Summon");
            Instantiate(normalTriangle, normalSpawnPoint.position, Quaternion.identity).layer = layer;
            Instantiate(normalTriangle, normalSpawnPoint.position, Quaternion.identity).layer = layer;
            yield return new WaitForSeconds(0.5f);
        }
        else if (rnd == 2)
        {
            // attack
            Debug.Log("Attack");
            rb.gravityScale = 1;

            Vector2 dir = (player.transform.position - transform.position).normalized;
            IsAttacking = true;
            rb.AddForce(dir * attackForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.5f);
            IsAttacking = false;



        }
        else if (rnd == 3)
        {
            // shockwave
            Debug.Log("Shock Attack");
            targetLaser.SetActive(true);
            Vector3 pos = transform.position;
            pos.z -= 2;
            targetLaser.transform.position = pos;
            Vector2 dir = (player.transform.position - transform.position);

            time = 0;
            while (time < 1.0f)
            {
                // ������ ���� : �÷��̾� ����
                
                // ������ �ʹ� �� ũ�� ������ ����
                //targetLaser.transform.localScale = new Vector3(dir.magnitude, 1, 1);
                targetLaser.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));

                pos = transform.position; pos.z -= 2;
                targetLaser.transform.position = pos;

                time += Time.deltaTime;
                yield return null;
                dir = (player.transform.position - transform.position);
            }
            yield return new WaitForSeconds(0.2f);
            targetLaser.SetActive(false);

            rb.gravityScale = 1;
            rb.sharedMaterial = bounceMaterial;

            shockWave.gameObject.SetActive(true);
            shockWave.Appear(transform.position);
            dir = (player.transform.position - transform.position).normalized;
            IsAttacking = true;
            rb.AddForce(dir * attackForce * 6f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(3f);
            IsAttacking = false;
            rb.sharedMaterial = null;

        }
        else if (rnd == 4)
        {
            // laser
            Debug.Log("Laser Attack");

            // ȸ�� ��Ȱ��ȭ
            rb.angularVelocity = 0;

            rb.AddTorque(2f);

            laserParent.gameObject.SetActive(true);
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].SetAlpha(0.3f);
                lasers[i].isLaserAttack = false;
            }

            yield return new WaitForSeconds(1f);
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].SetAlpha(1.0f);
                lasers[i].isLaserAttack = true;
            }
            

            yield return new WaitForSeconds(2f);

            laserParent.gameObject.SetActive(false);
        }

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        gameObject.layer = LayerMask.NameToLayer("IgnoreAll");
        time = 0;
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
        gameObject.layer = layer;
        spriteRenderer.color = Color.white;

        //yield return new WaitForSeconds(1.0f);


    }

    public override void TakeDamage(float damage)
    {

        base.TakeDamage(damage);

        if (step < 4 && health <= maxHp * 0.8f)
        {
            step = 4;
        }
        else if (step < 5 && health <= maxHp * 0.5f)
        {
            step = 5;
        }
        else if (summonRightTriangle == false && health <= maxHp * 0.3f)
        {
            summonRightTriangle = true;

            attackCoolDown /= 2f;

            Instantiate(rightTriangle, transform.position, Quaternion.identity);
            Instantiate(rightTriangle, transform.position, Quaternion.identity);

        }
    }

    protected override void Dead()
    {
        if (PlayerController.Instance == null)
            return;
        DeadEvent?.Invoke();
        Destroy(transform.parent.gameObject);
    }
}
