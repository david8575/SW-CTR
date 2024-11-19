using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightTriangleEnemy : MonoBehaviour
{
    public float speed = 1.0f;
    public float detectionRange = 5.0f;
    public float floatHeight = 2.0f;
    public float chargeSpeed = 6.0f;

    private Transform player;
    private bool isAlive = false;
    private bool isFloating = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isFloating) {
            // 플레이어 발견 시 떠오름
            StartCoroutine(FloatAndCharge());
        }
        else if (!isFloating) {
            // 기본 좌우 이동
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x > 3.0f || transform.position.x < -3.0f) 
            {
                speed = -speed; 
            }
        }
    }

    IEnumerator FloatAndCharge()
    {
        isFloating = true;
        Vector3 floatPosition = new Vector3(transform.position.x, transform.position.y + floatHeight, transform.position.z);

        // 천천히 떠오름
        while (Vector3.Distance(transform.position, floatPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, floatPosition, speed * Time.deltaTime);
            yield return null;
        }

        // 주인공을 향해 "/"형태로 빠르게 돌진
        Vector2 chargeDirection = (player.position - transform.position).normalized;
        transform.position += (Vector3)chargeDirection * chargeSpeed * Time.deltaTime;

        yield return new WaitForSeconds(0.5f); // 잠시 대기 후 공격 종료

        isFloating = false; // 다시 기본 패턴으로 복귀
    }

    public void TakeDamage() {
        if (!isAlive)
            return;

        isAlive = false;
        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            TakeDamage();
        }
    }
}
