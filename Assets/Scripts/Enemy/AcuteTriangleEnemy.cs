using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 5.0f;
    public float attackCoolTime = 2.0f;

    private Transform player;
    private bool isAttacking = false;
    private bool isAlive = true;
    private float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive){
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어를 향해 이동
        if (distanceToPlayer <= detectionRange){
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        
            // 공격 조건
            if (distanceToPlayer <= 1.0f && Time.time > lastAttackTime + attackCoolTime){
                StartCoroutine(AttackPlayer());
            }
        }
        // 플레이어 발견 전
        else {
            // 좌우 이동 로직
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x > 3.0f || transform.position.x < -3.0f) {
                speed = -speed; 
            }
        }
    }

    IEnumerator AttackPlayer(){
        isAttacking = true;
        lastAttackTime = Time.time;
        
        // 추후에 플레이어 체력 감소 로직 추가(아직 player관련 스크립트가 없어서)

        yield return new WaitForSeconds(attackCoolTime);
        isAttacking = false;
    }

    public void TakeDamage(){
        isAlive = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            TakeDamage();
        }
    }
}
