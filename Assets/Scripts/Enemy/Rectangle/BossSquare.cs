using System.Collections;
using UnityEngine;

public class BossSquare : EnemyBase, IHasDeadEvent
{
    [Header("Prefabs")]
    public GameObject miniSquarePrefab;
    public GameObject rightTriangle;
    public Transform miniSquareSpawnPoint; 
    public Prison prisonPrefab;

    [Header("Teleport Pattern")]
    public Transform[] teleportPoints;
    public Sprite brickSprite;

    private float maxHealth;
    public int step = 1;
    private SpriteRenderer spriteRenderer;
    LineRenderer line;

    public event System.Action DeadEvent = null;

    public string AlertSound = "Alert13";
    public string ExplosionSound = "explosion_21";
    public string LaserAlertSound = "Alert07";
    public string SummonSound = "Pickup_01";
    public string JumpSound = "jump_13";
    public string SizeChangeSound = "powerup_36";
    public string TeleportSound = "powerup_43";
    public string PrisonSummonSound = "hit_1";
    public string DashSound = "explosion_12";
    public string DestroySound = "Shoot17";

    protected override void Start()
    {
        base.Start();
        maxHealth = health;

        detectionRange = 1000f;
        attackRange = 1000f;

        spriteRenderer = GetComponent<SpriteRenderer>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    protected override void ApproachPlayer()
    {
        if (!IsAttacking)
            base.ApproachPlayer();
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
        IsAttacking = true;

        AudioManager.PlaySound(JumpSound);
        rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(new Vector2(0, -jumpPower * 2f), ForceMode2D.Impulse);
        AudioManager.PlaySound(ExplosionSound);
        yield return new WaitWhile(() => Mathf.Abs(rb.velocity.y) > 0.5f);
        IsAttacking = false;

        yield return new WaitForSeconds(1f);

        Debug.Log("Summon Mini Squares");
        AudioManager.PlaySound(SummonSound);
        Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
        
        yield return new WaitForSeconds(0.2f);
        AudioManager.PlaySound(SummonSound);
        Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);   
    }

    private IEnumerator TransformAttack()
    {
        Debug.Log("Transforming: Enlarging and Smashing");

        Vector3 originalScale = transform.localScale;
        Vector3 bigScale = new Vector3(originalScale.x * 8f, originalScale.y, originalScale.z);

        rb.velocity = Vector2.zero;
        AudioManager.PlaySound(JumpSound);
        rb.AddForce(Vector2.up * jumpPower * 2f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => rb.velocity.y <= 0);
        
        float time = 0;
        rb.gravityScale = 0;
        AudioManager.PlaySound(SizeChangeSound);
        while (time < 0.5f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, bigScale, 0.03f);
            time += Time.deltaTime;
            yield return null;
        }

        IsAttacking = true;
        rb.gravityScale = 1;
        rb.AddForce(Vector2.down * jumpPower * 5f, ForceMode2D.Impulse);
        AudioManager.PlaySound(ExplosionSound);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitWhile(() => Mathf.Abs(rb.velocity.y) > 0.5f);
        yield return new WaitForSeconds(0.5f);

        time = 0;
        AudioManager.PlaySound(SizeChangeSound);
        while (time < 0.5f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, 0.02f);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
        IsAttacking = false;

        Debug.Log("Smash attack completed and size reset.");
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator TeleportAttack()
    {
        Debug.Log("Teleporting and Summoning Mini Squares");
        AudioManager.PlaySound(SummonSound);
        for (int i = 0; i < 4; i++)
        {
            Instantiate(miniSquarePrefab, miniSquareSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }

        AudioManager.PlaySound(SummonSound);
        Instantiate(rightTriangle, miniSquareSpawnPoint.position, Quaternion.identity);
        var tri = Instantiate(rightTriangle, miniSquareSpawnPoint.position, Quaternion.identity);
        tri.transform.rotation = Quaternion.Euler(0, 0, 180);

        Sprite sprite = spriteRenderer.sprite;
        spriteRenderer.sprite = brickSprite;

        AudioManager.PlaySound(TeleportSound);
        Transform targetPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
        transform.position = targetPoint.position;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(3f);

        gameObject.tag = "Enemy";
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer.sprite = sprite;
    }

    private IEnumerator TrapPlayer()
    {
        Debug.Log("Trapping Player");
        AudioManager.PlaySound(AlertSound);
        yield return new WaitForSeconds(0.5f);
        Vector3 playerPosition = PlayerController.Instance.GetShapeTransform().position;

        AudioManager.PlaySound(PrisonSummonSound);
        Prison prison = Instantiate(prisonPrefab, playerPosition, Quaternion.identity);
        Transform[] pts = prison.points;

        yield return new WaitForSeconds(1f);
        rb.gravityScale = 0;
        line.enabled = true;
        IsAttacking = true;
        for (int i = 0; i < pts.Length + 1; i++)
        {
            rb.velocity = Vector2.zero;
            Vector3 target = teleportPoints[0].position;
            if (i != pts.Length)
            {
                target = pts[i].position;
            }
            float time = 0;

            AudioManager.PlaySound(LaserAlertSound);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, target);
            yield return new WaitForSeconds(0.7f);

            AudioManager.PlaySound(DashSound);
            while (time < 0.2f)
            {
                transform.position = Vector3.Lerp(transform.position, target, 0.04f);
                time += Time.deltaTime;
                yield return null;
            }
        }
        line.enabled = false;
        IsAttacking = false;
        rb.gravityScale = 1;

        AudioManager.PlaySound(DestroySound);
        Destroy(prison.gameObject);
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
    protected override void Dead()
    {
        if (PlayerController.Instance == null)
            return;
        DeadEvent?.Invoke();
        prisonPrefab.gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
    }
}
