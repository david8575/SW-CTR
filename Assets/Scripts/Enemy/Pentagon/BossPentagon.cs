using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPentagon : EnemyBase_old
{
    public GameObject miniPentagonPrefab;     
    public GameObject missilePrefab;           
    public GameObject strongTrianglePrefab;    
    public GameObject strongSquarePrefab;      
    public float dashSpeed = 20f;
    public float strongDashSpeed = 40f;

    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        attackPower = 15f;
        defense = 10f;
        moveSpeed = 5f;
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
        else if (health <= 0.3 * 100)
        {
            SummonStrongShapes(); 
        }
        else if (health <= 0.5 * 100)
        {
            StarPatternMissileAttack(); 
        }
        else if (health <= 0.8 * 100)
        {
            StrongDashAttack();
        }
        else
        {
            BasicAttack();
        }
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

        GameObject miniPentagon = Instantiate(miniPentagonPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        miniPentagon.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void StrongDashAttack()
    {
        if (canAttack)
        {
            StartCoroutine(StrongDashAttackRoutine());
        }
    }

    private IEnumerator StrongDashAttackRoutine()
    {
        canAttack = false;

        yield return new WaitForSeconds(2f);

        Vector2 direction = (player.transform.position - transform.position).normalized;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * strongDashSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void StarPatternMissileAttack()
    {
        if (canAttack)
        {
            StartCoroutine(StarPatternMissileRoutine());
        }
    }

    private IEnumerator StarPatternMissileRoutine()
    {
        canAttack = false;

        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        missile.GetComponent<Rigidbody2D>().velocity = direction * dashSpeed;

        yield return new WaitForSeconds(1f);
        FireAdditionalMissiles(missile.transform.position);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void FireAdditionalMissiles(Vector2 position)
    {
        GameObject missile1 = Instantiate(missilePrefab, position, Quaternion.identity);
        GameObject missile2 = Instantiate(missilePrefab, position, Quaternion.identity);

        Vector2 inwardDirection1 = new Vector2(-1, 1).normalized;
        Vector2 inwardDirection2 = new Vector2(1, 1).normalized;
        missile1.GetComponent<Rigidbody2D>().velocity = inwardDirection1 * dashSpeed;
        missile2.GetComponent<Rigidbody2D>().velocity = inwardDirection2 * dashSpeed;
    }

    private void SummonStrongShapes()
    {
        if (canAttack)
        {
            StartCoroutine(SummonStrongShapesRoutine());
        }
    }

    private IEnumerator SummonStrongShapesRoutine()
    {
        canAttack = false;

        Instantiate(strongTrianglePrefab, transform.position + Vector3.left * 2, Quaternion.identity);
        Instantiate(strongSquarePrefab, transform.position + Vector3.right * 2, Quaternion.identity);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
}
