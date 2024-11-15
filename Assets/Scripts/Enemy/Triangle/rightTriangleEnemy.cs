using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightTriangleEnemy : EnemyBase
{
    public float dashSpeed = 15f; 

    // Start is called before the first frame update
    void Start()
    {
        health = 30f;
        attackPower = 15f;
        defense = 10f;
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
        
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * dashSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
}
