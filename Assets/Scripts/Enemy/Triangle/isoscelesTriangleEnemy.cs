using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoscelesTriangleEnemy : EnemyBase
{
    public GameObject missilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        health = 30f;
        attackPower = 10f;
        defense = 5f;
        moveSpeed = 5f;
        jumpPower = 5f;
        attackCoolDown = 5f;
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
            DetectPlayer();

            if (isPlayerInRange)
            {
                ApproachPlayer();
                if (canAttack)
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    protected override IEnumerator Attack()
    {
        canAttack = false;

        FireMissile();

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void FireMissile()
    {
        if (missilePrefab != null)
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            missile.GetComponent<Rigidbody2D>().velocity = direction * 10f; 
        }
    }
}
