using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoscelesTriangleEnemy : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 5.0f;
    public float missileCoolTime = 3.0f;
    public GameObject missilePrefab;
    public Transform firePoint;

    private Transform player;
    private bool isAlive = true;
    private float lastMissileTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange) {
            // 플레이어를 향해 이동
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // 미사일 발사 조건 확인
            if (Time.time > lastMissileTime + missileCoolTime) {
                FireMissile();
            }
        }
        else {
            // 좌우 이동 로직
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x > 3.0f || transform.position.x < -3.0f) {
                speed = -speed; 
            }
        }
    }

    void FireMissile() {
        lastMissileTime = Time.time;
        
        // 미사일 생성 및 발사
        GameObject missile = Instantiate(missilePrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        missile.GetComponent<Rigidbody2D>().velocity = direction * 5.0f;
    }

    public void TakeDamage() {
        if (!isAlive){
            return;
        }
        isAlive = false;
        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")){
            TakeDamage();
        }
    }
}
