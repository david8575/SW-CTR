using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPentagon : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 7.0f;
    public float dashSpeed = 10.0f;
    public float attackCooldown = 3.0f;
    public float rotationSpeed = 360.0f; // 초당 회전 속도
    public GameObject glowStarPrefab; // 발광하는 별 프리팹 넣으면 될듯?
    public float glowDuration = 1.0f;

    private Transform player;
    private bool isDashing = false;
    private float nextAttackTime = 0;
    private Vector2 dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(GlowEffect());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && Time.time >= nextAttackTime)
        {
            StartCoroutine(RotateAndDash());
            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            Patrol();
        }
    }
    private void Patrol()
    {
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        if (transform.position.x > 3.0f || transform.position.x < -3.0f)
        {
            speed = -speed; // 방향 전환
        }
    }

    private IEnumerator RotateAndDash()
    {
        isDashing = true;

        // 플레이어 방향으로 돌진 준비
        dashDirection = (player.position - transform.position).normalized;

        // 회전하면서 돌진
        float dashTime = 0.5f;
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // 회전
            yield return null;
        }

        isDashing = false;
    }

    private IEnumerator GlowEffect()
    {
        while (true)
        {
            GameObject glowStar = Instantiate(glowStarPrefab, transform.position, Quaternion.identity, transform);
            glowStar.transform.localScale = Vector3.zero;

            float elapsed = 0f;
            while (elapsed < glowDuration)
            {
                // 발광 별이 커졌다가 작아지는 효과
                float scale = Mathf.PingPong(elapsed * 2 / glowDuration, 1.0f); // 0에서 1로 커졌다가 작아짐
                glowStar.transform.localScale = new Vector3(scale, scale, scale);
                elapsed += Time.deltaTime;
                yield return null;
            }
            Destroy(glowStar);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 추후 플레이어에게 데미지를 주는 로직 추가 
        }
    }
}
