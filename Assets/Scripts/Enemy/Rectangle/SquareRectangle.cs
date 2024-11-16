using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRectangle : EnemyBase_old
{
    public GameObject miniSquarePrefab;
    public GameObject missilePrefab;
    [SerializeField]
    public float mapWidth;
    private bool isRestrictingPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        health = 50f;
        attackPower = 20f;
        defense = 20f;
        moveSpeed = 1f;
        jumpPower = 1f;
        attackCoolDown = 5f;

        mapWidth = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        else if (health <= 0.3 * 100 && !isRestrictingPlayer)
        {
            StartCoroutine(LowHealthRestrictPlayer());
        }
        else if (health <= 0.3 * 100)
        {
            LongDiveAttack(0.75f);
        }
        else if (health <= 0.5 * 100)
        {
            StartCoroutine(TeleportAndShootMissile());
        }
        else if (health <= 0.8 * 100)
        {
            LongDiveAttack(0.5f); 
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

        GameObject miniSquare = Instantiate(miniSquarePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        miniSquare.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void LongDiveAttack(float scaleFactor)
    {
        if (canAttack)
        {
            StartCoroutine(LongDiveAttackRoutine(scaleFactor));
        }
    }

    private IEnumerator LongDiveAttackRoutine(float scaleFactor)
    {
        canAttack = false;

        Vector3 originalScale = transform.localScale;
        transform.localScale = new Vector3(mapWidth * scaleFactor, originalScale.y, originalScale.z);

        yield return new WaitForSeconds(0.5f);

        Vector2 direction = Vector2.down;
        GetComponent<Rigidbody2D>().AddForce(direction * attackPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);
        transform.localScale = originalScale;
        canAttack = true;
    }

    private IEnumerator TeleportAndShootMissile()
    {
        canAttack = false;

        Vector3 obstaclePosition = FindRandomObstaclePosition();
        transform.position = obstaclePosition;

        yield return new WaitForSeconds(1f);
        ShootMissileAtPlayer();

        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void ShootMissileAtPlayer()
    {
        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        missile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
    }

    private Vector3 FindRandomObstaclePosition()
    {
        return new Vector3(Random.Range(-mapWidth / 2, mapWidth / 2), transform.position.y, transform.position.z);
    }

    private IEnumerator LowHealthRestrictPlayer()
    {
        isRestrictingPlayer = true;

        GameObject restrictionBox = new GameObject("RestrictionBox");
        restrictionBox.transform.position = player.transform.position;
        restrictionBox.transform.localScale = new Vector3(5f, 5f, 1f);

        while (restrictionBox.transform.localScale.x > 1f)
        {
            restrictionBox.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(restrictionBox);
        isRestrictingPlayer = false;
    }
}
