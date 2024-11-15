using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class equilateralTriangleEnemy : EnemyBase
{
    // 여기 프리팹들에 대해서 스크립트를 따로 만들어야 하는지 
    // 아님 여기서 구현하면 되는지
    // 직각 삼각형 소환체들은 rightTriangle.cs쓰면 될 듯 합니다
    // 만약에 스크립트 따로 필요하면 말해주세요
    public GameObject miniTrianglePrefab; 
    public GameObject shockwavePrefab;
    public GameObject laserPrefab;
    public GameObject rightTrianglePrefab;

    // Start is called before the first frame update
    void Start()
    {
        health = 50f;
        attackPower = 20f;
        defense = 10f;
        moveSpeed = 10f;
        jumpPower = 10f;
        attackCoolDown = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        else if (health <= 0.3*50)
        {
            LowHealthSummon();   
        }
        else if (health <= 0.5*50)
        {
            MidHealthLaser();   
        }
        else if (health <= 0.8*50)
        {
            HighHealthShockwave();   
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

        GameObject miniTriangle = Instantiate(miniTrianglePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        miniTriangle.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void HighHealthShockwave()
    {
        if (canAttack)
        {
            StartCoroutine(ShockwaveAttack());
        }
    }

    private IEnumerator ShockwaveAttack()
    {
        canAttack = false;

        GameObject shockwaveInstance = Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
        Destroy(shockwaveInstance, 3f);

        Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * moveSpeed * 1.5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void MidHealthLaser()
    {
        if (canAttack)
        {
            StartCoroutine(LaserAttack());
        }
    }

    private IEnumerator LaserAttack()
    {
        canAttack = false;

        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        laser.transform.SetParent(transform); 
        yield return new WaitForSeconds(0.1f);

        float rotateDuration = 5f;
        float rotateSpeed = 20f;
        for (float t = 0; t < rotateDuration; t += Time.deltaTime)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        canAttack = true;
    }

    private void LowHealthSummon()
    {
        if (canAttack)
        {
            StartCoroutine(SummonRightTriangles());
        }
    }

    private IEnumerator SummonRightTriangles()
    {
        canAttack = false;

        Instantiate(rightTrianglePrefab, transform.position + Vector3.left * 1.5f, Quaternion.identity);
        Instantiate(rightTrianglePrefab, transform.position + Vector3.right * 1.5f, Quaternion.identity);

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
}
