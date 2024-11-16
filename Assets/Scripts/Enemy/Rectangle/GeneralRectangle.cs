using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRectangle : EnemyBase_old
{
    // Start is called before the first frame update
    void Start()
    {
        health = 50f;
        attackPower = 20f;
        defense = 20f;
        moveSpeed = 1f;
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

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y+10f, transform.position.z);
        
        yield return new WaitForSeconds(0.5f);
        Vector2 downDirection = Vector2.down;
        GetComponent<Rigidbody2D>().AddForce(downDirection * attackPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }  
}
