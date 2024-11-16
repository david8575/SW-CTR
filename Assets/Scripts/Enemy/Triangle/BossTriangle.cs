using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriangle : EnemyBase
{

    // Wait, Attack, Summon triangle
    // 80% hp: Shokwave Attack
    // 50% hp: Laser Attack
    // 30% hp: Summon Right Triangle

    [SerializeField]
    int step = 3;
    float attackForce = 10f;
    float maxHp;

    public PhysicsMaterial2D bounceMaterial;

    [Header("SpawnPoints")]
    public Transform normalSpawnPoint; 

    [Header("Prefabs")]
    public GameObject normalTriangle;
    public GameObject shockWave;

    protected override void Start()
    {
        base.Start();
        maxHp = health;
    }


    protected override void ApproachPlayer()
    {
        //base.ApproachPlayer();
    }


    protected override IEnumerator Attack()
    {
        int rnd = Random.Range(0, step);

        if (rnd == 0)
        {
            // wait
            yield return new WaitForSeconds(1.0f);
            Debug.Log("Wait");
        }
        else if (rnd == 1)
        {
            // attack
            Debug.Log("Attack");
            Vector2 dir = (player.transform.position - transform.position).normalized;
            isAttacking = true;
            rb.AddForce(dir * attackForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }
        else if (rnd == 2)
        {
            // summon
            Debug.Log("Summon");
            GameObject miniTriangle = Instantiate(normalTriangle, normalSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        else if (rnd == 3)
        {
            // shockwave
            Debug.Log("Shock Attack");
            
            rb.sharedMaterial = bounceMaterial;

            Instantiate(shockWave, transform.position, Quaternion.identity);
            Vector2 dir = (player.transform.position - transform.position).normalized;
            isAttacking = true;
            rb.AddForce(dir * attackForce * 3f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1f);
            isAttacking = false;
            rb.sharedMaterial = null;
        }
        else if (rnd == 4)
        {
            // laser
        }
        else if (rnd == 5)
        {
            // summon right triangle
        }

        yield return null;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health <= maxHp * 0.8f)
        {
            step = 4;
        }
        else if (health <= maxHp * 0.5f)
        {
            step = 5;
        }
        else if (health <= maxHp * 0.3f)
        {
            step = 6;
        }
    }
}
