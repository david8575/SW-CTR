using System.Collections;
using UnityEngine;

public class SquareRectangle : EnemyBase
{
    [Header("Square Rectangle Settings")]
    public GameObject miniSquarePrefab; 
    public Transform miniSquareSpawnPoint; 
    public GameObject prisonPrefab; 
    public Transform[] teleportPoints; 

    private float maxHealth;
    private bool hasTransformed = false;
    private bool hasTeleported = false;
    private int step = 1; 

    protected override void Start()
    {
        base.Start();
        maxHealth = health;

        step = 4;
    }

    protected override IEnumerator Attack()
    {
        int rnd = Random.Range(0, step); 

        if (rnd == 0)
        {
            yield return StartCoroutine(BasicAttack());
        }
        else if (rnd == 1)
        {
            yield return StartCoroutine(TransformAttack());
        }
        else if (rnd == 2)
        {
            yield return StartCoroutine(TeleportAttack());
        }
        else if (rnd == 3)
        {
            yield return StartCoroutine(TrapPlayer());
        }
    }

    private IEnumerator BasicAttack()
    {
        Debug.Log("Basic Attack: Jump and Smash");

        
        Vector3 targetPosition = PlayerController.Instance.transform.position;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(new Vector2(0, -jumpPower * 2f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Summon Mini Squares");
        Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
    }

    private IEnumerator TransformAttack()
    {
        if (!hasTransformed)
        {
            Debug.Log("Transforming: Enlarging and Smashing");
            hasTransformed = true;

            Vector3 originalScale = transform.localScale;
            transform.localScale = new Vector3(originalScale.x * 2f, originalScale.y, originalScale.z);

            yield return new WaitForSeconds(0.5f);

            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            yield return new WaitUntil(() => rb.velocity.y <= 0); 

            IsAttacking = true;
            rb.AddForce(Vector2.down * jumpPower * 3f, ForceMode2D.Impulse);

            yield return new WaitUntil(() =>
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Ground"))
                    {
                        return true;
                    }
                }
                return false;
            });

            transform.localScale = originalScale;
            IsAttacking = false;

            Debug.Log("Smash attack completed and size reset.");
            yield return new WaitForSeconds(0.5f);
        }

        hasTransformed = false;
    }

    private IEnumerator TeleportAttack()
    {
        if (!hasTeleported)
        {
            Debug.Log("Teleporting and Summoning Mini Squares");
            hasTeleported = true;

            for (int i = 0; i < 3; i++)
            {
                Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(0.3f);
            }

            Transform targetPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
            transform.position = targetPoint.position;
            yield return new WaitForSeconds(0.5f);
        }
        hasTeleported = false;
    }

    private IEnumerator TrapPlayer()
    {
        Debug.Log("Trapping Player");

        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;

        GameObject prison = Instantiate(prisonPrefab, playerPosition, Quaternion.identity);

        prison.transform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);

        Debug.Log($"Player Position: {PlayerController.Instance.transform.position}");

        yield return new WaitForSeconds(3f);

        Destroy(prison);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (step < 2 && health <= maxHealth * 0.8f)
        {
            step = 2;
        }
        else if (step < 3 && health <= maxHealth * 0.5f)
        {
            step = 3;
        }
        else if (step < 4 && health <= maxHealth * 0.3f)
        {
            step = 4;
        }
    }
}
