using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenaralPentagon : EnemyBase_old
{
    public float dashSpeed = 40f;
    public float rotateSpeed = 360f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        health = 30f;
        attackPower = 10f;
        defense = 5f;
        moveSpeed = 10f;
        jumpPower = 1f;
        attackCoolDown = 5f;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; 
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

        spriteRenderer.color = Color.yellow;
        StartCoroutine(RotateWhileDashing());

        yield return new WaitForSeconds(1f);

        Vector2 direction = (player.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * dashSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolDown);

        spriteRenderer.color = originalColor; 
        canAttack = true;
    }

    private IEnumerator RotateWhileDashing()
    {
        float dashDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
