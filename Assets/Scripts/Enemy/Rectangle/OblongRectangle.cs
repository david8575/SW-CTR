using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class OblongRectangle : EnemyBase
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

        UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(player.transform.position.x, player.transform.position.y + 3f, transform.position.z);
        transform.position = targetPosition;

        UnityEngine.Vector3 originalScale = transform.localScale;  
        transform.localScale = new UnityEngine.Vector3(originalScale.x * 2, originalScale.y, originalScale.z);

        yield return new WaitForSeconds(0.5f);

        UnityEngine.Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * attackPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        transform.localScale = originalScale;
        canAttack = true;
    }
}
