using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPentagon : EnemyBase
{
    [Header("Boss Specific")]
    public GameObject miniPentagonPrefab;
    public GameObject trianglePrefab;
    public GameObject squarePrefab;
    public GameObject missilePrefab;
    public Transform[] cornerPositions;

    [Header("Missile Settings")]
    public Transform missileSpawnPoint;

    public string SummonSound = "Pickup_01";
    public string SummonSound2 = "Pickup_00";
    public string JumpSound = "jump_13";
    public string DashSound = "Shoot17";
    public string DashSound2 = "explosion_12";
    public string MissileSound = "Hit7";

    protected override IEnumerator Attack()
    {
        int randomAttack = Random.Range(1, 6); // 1~5 랜덤 선택
        Debug.Log($"BossPentagon performing attack {randomAttack}");

        switch (randomAttack)
        {
            case 1: // 돌진 공격
                yield return RushAttack();
                break;
            case 2: // 미니 오각형 소환
                yield return SummonMiniPentagons();
                break;
            case 3: // 강한 돌진 공격
                yield return StrongRushAttack();
                break;
            case 4: // 꼭짓점에서 미사일 발사
                // yield return MissileFromCorners();
                // break;
            case 5: // 삼각형과 사각형 소환
                yield return SummonShapes();
                break;
        }
    }

    private IEnumerator RushAttack()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        AudioManager.PlaySound(DashSound);
        rb.AddForce(direction * moveSpeed * 10f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
    }

    private IEnumerator SummonMiniPentagons()
    {
        AudioManager.PlaySound(SummonSound);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(miniPentagonPrefab, transform.position + (Vector3)(Random.insideUnitCircle * 2f), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator StrongRushAttack()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        AudioManager.PlaySound(DashSound2);
        rb.AddForce(direction * moveSpeed * 20f, ForceMode2D.Impulse);

        for (int i = 0; i < 5; i++) // 벽에 튕기는 효과
        {
            yield return new WaitForSeconds(0.5f);
            Vector2 bounceDirection = Vector2.Reflect(rb.velocity.normalized, Random.insideUnitCircle.normalized);
            rb.AddForce(bounceDirection * moveSpeed * 10f, ForceMode2D.Impulse);
        }

        rb.velocity = Vector2.zero;
    }

    private IEnumerator MissileFromCorners()
    {
        for (int i = 0; i < 5; i++) // 5개의 미사일 발사
    {
        AudioManager.PlaySound(MissileSound);
        // 미사일 생성
        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
        
        // 미사일이 플레이어를 향하도록 방향 설정
        Vector2 direction = (player.position - missileSpawnPoint.position).normalized;
        Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();
        missileRb.velocity = direction * 10f; // 미사일 속도 설정
        
        yield return new WaitForSeconds(0.2f); // 발사 간격
    }
    }

    private IEnumerator SummonShapes()
    {
        AudioManager.PlaySound(SummonSound2);
        Instantiate(trianglePrefab, transform.position + Vector3.left * 2, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        AudioManager.PlaySound(SummonSound2);
        Instantiate(squarePrefab, transform.position + Vector3.right * 2, Quaternion.identity);
    }
}
