using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octagon : EnemyBase_old
{
    
    private bool DefendBoost = false;
    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        attackPower = 15f;
        defense = 10f;
        moveSpeed = 3f;
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
        else if (health <= 0.5 * 100 && !DefendBoost)
        {
            DefenseBoost();
        }

        BasicAttack();
    
    }

    private void BasicAttack()
    {
        if (canAttack)
        {
            StartCoroutine(BasicAttackRoutine());
        }
    }

    private IEnumerator BasicAttackRoutine()
    {
        canAttack = false;

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y+10f, transform.position.z);
        
        yield return new WaitForSeconds(0.5f);
        Vector2 downDirection = Vector2.down;
        GetComponent<Rigidbody2D>().AddForce(downDirection * attackPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void DefenseBoost()
    {
        DefendBoost = true;
        float originalDefense = defense;
        defense *= 2;
    }
}
