using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircle : EnemyBase
{
    [Header("Circle Specific")]
    public GameObject[] enemyPrefabs; // 랜덤 소환할 적 프리팹
    public GameObject[] bossPrefabs;

    [Header("Laser")]
    public Laser[] laserPrefab; // 레이저
    public Sprite laserFormSprite;

    private Sprite defaultSprite;
    [Header("Charge and Rush")]
    public Transform ChargeParent;
    Transform fireEffect;
    Transform chargeEffect;
    public GameObject targetLaser;
    public PhysicsMaterial2D bounceMaterial;

    [Header("Big attack")]
    public Transform attackWarning;
    public Transform mapCenter; // 맵 중심 위치

    [Header("Missile Attack")]
    public GameObject missilePrefab; // 미사일 프리팹
    public BossCircleLine lineRenderer;

    float maxHealth;
    float firstAttackCoolDown;
    private SpriteRenderer spriteRenderer;
    int flag = 0;
    int layerNumber;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        hpBar.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;

        fireEffect = ChargeParent.Find("sidefire2");
        chargeEffect = ChargeParent.Find("charge");

        maxHealth = health;
        firstAttackCoolDown = attackCoolDown;

        detectionRange = 1000f;
        attackRange = 1000f;

        layerNumber = LayerMask.NameToLayer("Enemy");
    }

    protected override void ApproachPlayer() { }

    protected override IEnumerator Attack()
    {
        int randomAttack = Random.Range(25, 100); // 확률 선택

        if (randomAttack < 25)
        {
            yield return StartCoroutine(SummonRandomEnemy());
        }
        else if (randomAttack < 45)
        {
            yield return StartCoroutine(ChargeAndRush());
            
        }
        else if (randomAttack < 60)
        {
            yield return StartCoroutine(BounceRush());
            
        }
        else if (randomAttack < 75)
        {
            yield return StartCoroutine(TransformAndLaser());
        }
        else if (randomAttack < 90)
        {
            yield return StartCoroutine(TransformAndShoot());
        }
        else
        {
            yield return StartCoroutine(GrowAtCenter());
        }
        yield return StartCoroutine(SummonRandomEnemy());
    }

    private IEnumerator SummonRandomEnemy()
    {
        Debug.Log("Summoning random enemy and Wait");
        GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        var s = Instantiate(randomEnemy, transform.position + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity);
        s.layer = layerNumber;
        yield return new WaitForSeconds(0.3f);
    }

    private IEnumerator GrowAtCenter()
    {
        Debug.Log("Growing at center");
        
        yield return StartCoroutine(GoCenter());
        attackWarning.position = mapCenter.position;
        attackWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        float alp = 0;
        Vector3 originalScale = transform.localScale;
        IsAttacking = true;
        attackImage.SetActive(false);
        // 커지면서 등장
        while (alp < 1)
        {
            alp += Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, alp);
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one * 10f, 0.03f);
            yield return null;
        }
        attackWarning.gameObject.SetActive(false);

        // 일정 시간 대기
        yield return new WaitForSeconds(2f);

        // 원래 크기로 돌아가기
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, 0.01f);
            yield return null;
        }
        IsAttacking = false;
        transform.localScale = originalScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private IEnumerator TransformAndShoot()
    {
        Debug.Log("TransformAndShoot");
        yield return StartCoroutine(GoCenter());
        yield return new WaitForSeconds(0.5f);
        float alp = 0;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        while (alp < 1)
        {
            alp += Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, alp);
            yield return null;
        }
        lineRenderer.gameObject.SetActive(true);

        int fireCount = Random.Range(5, 10);
        for (int i = 0; i < fireCount; i++)
        {
            var missile = Instantiate(missilePrefab, lineRenderer.movingTaget.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }

        lineRenderer.gameObject.SetActive(false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator TransformAndLaser()
    {
        Debug.Log("TransformAndLaser");
        
        yield return StartCoroutine(GoCenter());
        yield return new WaitForSeconds(0.5f);

        float alp = 0;
        spriteRenderer.sprite = laserFormSprite;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        while (alp < 1)
        {
            alp += Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, alp);
            yield return null;
        }


        // 레이저 발사
        for (int i = 0; i < laserPrefab.Length; i++)
        {
            laserPrefab[i].gameObject.SetActive(true);
            laserPrefab[i].SetAlpha(0.3f);
            laserPrefab[i].isLaserAttack = false;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < laserPrefab.Length; i++)
        {
            laserPrefab[i].SetAlpha(1.0f);
            laserPrefab[i].isLaserAttack = true;
        }

        // 한바퀴 회전
        float time = 0;
        while (time < 3f)
        {
            transform.Rotate(Vector3.forward, 45 * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        // 레이저 끄고 복귀
        for (int i = 0; i < laserPrefab.Length; i++)
        {
            laserPrefab[i].gameObject.SetActive(false);
        }
        spriteRenderer.sprite = defaultSprite; // 원래 모습으로 복귀
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ChargeAndRush()
    {
        Debug.Log("ChargeAndRush");
        float time = 0;
        Vector3 two = new Vector3(2, 2, 2);
        chargeEffect.localScale = two;
        chargeEffect.gameObject.SetActive(true);
        // 차징 시작
        while (time < 1.5f)
        {
            time += Time.deltaTime;
            chargeEffect.localScale = Vector3.Lerp(two, Vector3.zero, time / 1.5f);
            yield return null;
        }
        chargeEffect.gameObject.SetActive(false);
        Vector2 direction = (player.position - transform.position).normalized;
        rb.AddForce(direction * moveSpeed, ForceMode2D.Impulse);

        time = 0;
        fireEffect.gameObject.SetActive(true);
        IsAttacking = true;
        while (time < 2f)
        {
            Vector3 myDir = rb.velocity.normalized;
            // 오른쪽 0도, 왼쪽 이동 중일땐 180도
            fireEffect.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(myDir.y, myDir.x) * Mathf.Rad2Deg);
            time += Time.deltaTime;
            yield return null;
        }
        fireEffect.gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        IsAttacking = false;
    }

    private IEnumerator BounceRush()
    {
        Debug.Log("BounceRush");

        // 조준하기
        targetLaser.SetActive(true);
        Vector3 pos = transform.position;
        pos.z -= 2;
        targetLaser.transform.position = pos;
        Vector2 dir = (player.transform.position - transform.position).normalized;

        float time = 0;
        while (time < 1.0f)
        {
            //targetLaser.transform.localScale = new Vector3(dir.magnitude, 1, 1);
            targetLaser.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));

            pos = transform.position; pos.z -= 2;
            targetLaser.transform.position = pos;

            time += Time.deltaTime;
            yield return null;
            dir = (player.transform.position - transform.position).normalized;
        }
        yield return new WaitForSeconds(1f);
        targetLaser.SetActive(false);

        rb.constraints = RigidbodyConstraints2D.None;
        // 발사하기
        rb.AddForce(dir * moveSpeed * 2f, ForceMode2D.Impulse);
        IsAttacking = true;

        var matSave = rb.sharedMaterial;
        rb.sharedMaterial = bounceMaterial;
        yield return new WaitForSeconds(3f);
        rb.sharedMaterial = matSave;
        IsAttacking = false;

        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // 체력 비율만큼 쿨다운 감소
        attackCoolDown = firstAttackCoolDown * (health / maxHealth);
        if (flag < 1 && health <= maxHealth * 0.7f)
        {
            flag = 1;

            // summon boss Triangle
            var tri = Instantiate(bossPrefabs[0], transform.position + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).
                transform.GetChild(0).GetComponent<BossTriangle>();

            tri.step = 5;
            tri.gameObject.layer = layerNumber;
        }
        else if (flag < 2 && health <= maxHealth * 0.4f)
        {
            flag = 2;
            // summon boss Square
            var rect = Instantiate(bossPrefabs[1], transform.position + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).
                transform.GetChild(0).GetComponent<BossSquare>();
            rect.step = 4;
        }
    }

    /// <summary>
    /// 사라진 뒤에 맵 가운데로 이동 (모든 이동 비활성된 상태)
    /// </summary>
    /// <returns></returns>
    IEnumerator GoCenter()
    {
        float alp = 1;
        while (alp > 0)
        {
            alp -= Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, alp);
            yield return null;
        }
        transform.position = mapCenter.position;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
    }
}
