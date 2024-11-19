using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcuteTriangleEnmey : MonoBehaviour
{
    public float speed = 4.0f;
    public float detectionRange = 5.0f;
    public float attackCoolTime = 2.0f;

    public bool isAttacking = false;
    public bool canAttack = true;

    private bool isAlive = true;
    private float lastAttackTime;

    float time = 0;

    /// <summary>
    /// 1. 적의 경우 인터페이스든 Enemy 상위 클래스든 모든 적을 포괄하는 무언가가 필요합니다. 
    ///     일단 이번에는 그냥 이 스크립트만 있다고 가정하고 만들었습니다. (정보 가져오기용)
    /// 2. 적이 플레이어를 인식하는 거리가 너무 짧습니다. (5.0f로 수정)
    /// 3. 적이 움직이긴 하는데 0.1cm 움직이는 걸로 확인되어서 아예 수정하였습니다.
    /// 4. rigidbody의 중력을 0으로 해놓으면 충돌 후 둥둥 떠다녀서 일단 씬에서는 중력 값을 올려서 테스트했습니다.
    /// 5. 플레이어 도형의 경우 도형을 바뀔때마다 오브젝트 자체가 달라지기 때문에 매번 확인하도록 했습니다.
    /// 6. enemy 태그 만들어놓았으니 모든 적에게 설정해주시기 바랍니다.
    /// 7. 파일 이름 바꿔도 클래스 이름은 안바뀝니다. 여기 NewBehaviourScript로 되어있는거 수정했습니다.
    /// </summary>


    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }

        // 플레이어 도형이 계속 바뀌어서 매번 가져오도록 수정
        Transform player = PlayerController.Instance.ShapeInfo.transform;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        

        // 플레이어를 향해 이동
        if (distanceToPlayer <= detectionRange){
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
            
            // 공격 조건
            if (canAttack && distanceToPlayer <= 3.0f){
                StartCoroutine(AttackPlayer());
            }
        }
        // 플레이어 발견 전
        else {
            // 시간에 따른 좌우 이동으로 수정
            time += Time.fixedDeltaTime;
            // 좌우 이동 로직
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            if (time > 3f) {
                speed = -speed;
                time = 0;
            }
        }
    }

    IEnumerator AttackPlayer(){
        isAttacking = true;
        canAttack = false;

        Transform player = PlayerController.Instance.ShapeInfo.transform;
        Vector2 dir = (player.position - transform.position).normalized;

        GetComponent<Rigidbody2D>().AddForce(dir * 10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCoolTime);
        isAttacking = false;

        yield return new WaitForSeconds(attackCoolTime);
        canAttack = true;
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
