using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircle : EnemyBase_old
{
    public GameObject missilePrefab;    
    public GameObject laserPrefab;        
    public GameObject triangleEnemyPrefab; 
    public GameObject squareEnemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        health = 200f;
        attackPower = 10f;
        defense = 15f;
        moveSpeed = 3f;
        jumpPower = 1f;
        attackCoolDown = 5f;

        StartCoroutine(RandomAttackRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RandomAttackRoutine()
    {
        while (health > 0)
        {
            if (canAttack)
            {
                int randomPattern = Random.Range(0, 5);
                switch (randomPattern)
                {
                    case 0:
                        StartCoroutine(ChargeAttack());
                        break;
                    case 1:
                        SummonRandomEnemy();
                        break;
                    case 2:
                        StartCoroutine(EnlargeTemporarily());
                        break;
                    case 3:
                        FireMissilesFromTriangle();
                        break;
                    case 4:
                        FireLasersFromSquare();
                        break;
                }
            }
            yield return new WaitForSeconds(attackCoolDown);
        }
    }

    private IEnumerator ChargeAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f); 

        // 주인공을 향해 돌진
        Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * attackPower * 5, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void SummonRandomEnemy()
    {
        canAttack = false;

        GameObject randomEnemyPrefab = Random.Range(0, 2) == 0 ? triangleEnemyPrefab : squareEnemyPrefab;
        Instantiate(randomEnemyPrefab, transform.position + Vector3.up * 2, Quaternion.identity);

        canAttack = true;
    }

    private IEnumerator EnlargeTemporarily()
    {
        canAttack = false;

        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.5f;
        yield return new WaitForSeconds(3f);
        transform.localScale = originalScale;

        canAttack = true;
    }

    private void FireMissilesFromTriangle()
    {
        canAttack = false;

        Vector3[] triangleVertices = new Vector3[]
        {
            transform.position + Vector3.up * 1.5f,
            transform.position + new Vector3(-1.3f, -0.75f, 0),
            transform.position + new Vector3(1.3f, -0.75f, 0)
        };

        foreach (Vector3 vertex in triangleVertices)
        {
            GameObject missile = Instantiate(missilePrefab, vertex, Quaternion.identity);
            Vector2 direction = (player.transform.position - vertex).normalized;
            missile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
        }

        canAttack = true;
    }

    private void FireLasersFromSquare()
    {
        canAttack = false;

        Vector3[] squareVertices = new Vector3[]
        {
            transform.position + Vector3.up * 2f,
            transform.position + Vector3.down * 2f,
            transform.position + Vector3.left * 2f,
            transform.position + Vector3.right * 2f
        };

        foreach (Vector3 vertex in squareVertices)
        {
            GameObject laser = Instantiate(laserPrefab, vertex, Quaternion.identity);
            Vector2 direction = (player.transform.position - vertex).normalized;
            laser.GetComponent<Rigidbody2D>().velocity = direction * 10f;
        }

        canAttack = true;
    }
}
