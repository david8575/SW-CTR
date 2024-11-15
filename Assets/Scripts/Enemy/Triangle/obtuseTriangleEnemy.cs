using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obtuseTriangleEnemy : EnemyBase
{
    public float dashSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        health = 30f;
        attackPower = 10f;
        defense = 5f;
        moveSpeed = 10f;
        jumpPower = 1f;
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

        yield return new WaitForSeconds(1f);

        Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * dashSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
}
