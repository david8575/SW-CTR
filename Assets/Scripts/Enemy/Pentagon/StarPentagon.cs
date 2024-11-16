using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPentagon : EnemyBase_old
{
    public float dashSpeed = 40f;
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
    
}
