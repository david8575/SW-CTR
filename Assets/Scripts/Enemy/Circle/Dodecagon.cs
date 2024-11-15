using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodecagon : EnemyBase
{
    public GameObject flamePrefab;
    public float flameDuration = 3f;
    public float flameSpawnRate = 0.2f;
    public float rotationSpeed = 360f;
    public float specialDashSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        health = 120f;
        attackPower = 20f;
        defense = 15f;
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
        else
        {
            DetectPlayer();

            if (isPlayerInRange && canAttack)
            {
                if (health <= 0.5 * 120)
                {
                    StartCoroutine(SpecialFlameDash());
                }
                else
                {
                    StartCoroutine(BasicDash());
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    private IEnumerator BasicDash()
    {
        canAttack = false;

        yield return new WaitForSeconds(0.5f);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * moveSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private IEnumerator SpecialFlameDash()
    {
        canAttack = false;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SpawnFlames());

        Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * specialDashSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private IEnumerator SpawnFlames()
    {
        float spawnDuration = 2f; 
        float elapsedTime = 0f;

        while (elapsedTime < spawnDuration)
        {
            GameObject flame = Instantiate(flamePrefab, transform.position, Quaternion.identity);
            Destroy(flame, flameDuration); 

            yield return new WaitForSeconds(flameSpawnRate);
            elapsedTime += flameSpawnRate;
        }
    }
}
