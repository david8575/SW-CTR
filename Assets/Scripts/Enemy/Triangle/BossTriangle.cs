using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossTriangle : EnemyBase
{

    // Wait, Attack, Summon triangle
    // 80% hp: Shokwave Attack
    // 50% hp: Laser Attack
    // 30% hp: Summon Right Triangle
    bool summonRightTriangle = false;

    [SerializeField]
    int step = 3;
    float attackForce = 10f;
    float maxHp;

    [SerializeField]
    Vector3 startPos;

    public PhysicsMaterial2D bounceMaterial;

    [Header("SpawnPoints")]
    public Transform normalSpawnPoint;

    [Header("Prefabs")]
    public GameObject normalTriangle;
    public ShockWave shockWave;
    public GameObject targetLaser;
    public GameObject rightTriangle;

    [Header("Laser")]
    public Laser[] lasers;
    public Transform laserParent;

    [Header("UI")]
    public Image hpBar;
    public TextMeshProUGUI hpText;

    protected override void Start()
    {
        base.Start();
        maxHp = health;

        startPos = transform.position;

        laserParent.gameObject.SetActive(false);
    }


    protected override void ApproachPlayer()
    {
        //base.ApproachPlayer();
    }


    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);

        int rnd = Random.Range(0, step);
        float time;

        if (rnd == 0)
        {
            // wait
            yield return new WaitForSeconds(1.0f);
            Debug.Log("Wait");

            yield break;
        }
        else if (rnd == 1)
        {
            // summon
            Debug.Log("Summon");
            GameObject miniTriangle = Instantiate(normalTriangle, normalSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);

            yield break;
        }

        if (rnd == 2)
        {
            // attack
            Debug.Log("Attack");
            rb.gravityScale = 1;
            Vector2 dir = (player.transform.position - transform.position).normalized;
            isAttacking = true;
            rb.AddForce(dir * attackForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.5f);
            isAttacking = false;


        }
        else if (rnd == 3)
        {
            // shockwave
            Debug.Log("Shock Attack");
            targetLaser.SetActive(true);
            Vector3 pos = transform.position;
            pos.z -= 2;
            targetLaser.transform.position = pos;
            Vector2 dir = (player.transform.position - transform.position);

            time = 0;
            while (time < 1.0f)
            {
                // 레이저 방향 : 플레이어 방향
                
                // 레이저 너무 길어서 크기 조절은 안함
                //targetLaser.transform.localScale = new Vector3(dir.magnitude, 1, 1);
                targetLaser.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));

                pos = transform.position; pos.z -= 2;
                targetLaser.transform.position = pos;

                time += Time.deltaTime;
                yield return null;
                dir = (player.transform.position - transform.position);
            }
            targetLaser.SetActive(false);

            rb.gravityScale = 1;
            rb.sharedMaterial = bounceMaterial;

            shockWave.gameObject.SetActive(true);
            shockWave.Appear(transform.position);
            dir = (player.transform.position - transform.position).normalized;
            isAttacking = true;
            rb.AddForce(dir * attackForce * 4f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(3f);
            isAttacking = false;
            rb.sharedMaterial = null;
        }
        else if (rnd == 4)
        {
            // laser
            Debug.Log("Laser Attack");

            laserParent.gameObject.SetActive(true);
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].SetAlpha(0.3f);
            }

            yield return new WaitForSeconds(0.7f);
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].SetAlpha(1.0f);
                lasers[i].isLaserAttack = true;
            }

            yield return new WaitForSeconds(2f);

            laserParent.gameObject.SetActive(false);
        }

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        gameObject.layer = LayerMask.NameToLayer("IgnoreAll");
        time = 0;
        while (Vector2.Distance(transform.position, startPos) > 0.3f)
        {
            transform.position = Vector2.Lerp(transform.position, startPos, 0.1f);
            yield return new WaitForSeconds(0.05f);

            time += Time.deltaTime;
            if (time > 2.0f)
            {
                break;
            }
        }
        transform.position = startPos;
        gameObject.layer = LayerMask.NameToLayer("Default");

        //yield return new WaitForSeconds(1.0f);


    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (step < 4 && health <= maxHp * 0.8f)
        {
            step = 4;
        }
        else if (step < 5 && health <= maxHp * 0.5f)
        {
            step = 5;
        }
        else if (summonRightTriangle == false && health <= maxHp * 0.3f)
        {
            summonRightTriangle = true;

            Instantiate(rightTriangle, normalSpawnPoint.position, Quaternion.identity);
            Instantiate(rightTriangle, normalSpawnPoint.position, Quaternion.identity);

        }

        hpBar.fillAmount = health / maxHp;
        hpText.text = health + " / " + maxHp;
    }
}
