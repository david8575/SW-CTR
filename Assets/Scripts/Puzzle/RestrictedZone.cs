using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedZone : MonoBehaviour
{
    [Header("Zone Settings")]
    public string playerTag = "Player"; // 플레이어의 태그
    public string enemyTag = "Enemy";   // 적의 태그

    private Collider2D zoneCollider;

    private void Start()
    {
        // Collider2D 컴포넌트 확인
        zoneCollider = GetComponent<Collider2D>();
        if (zoneCollider == null)
        {
            Debug.LogError("RestrictedZone requires a Collider2D component.");
            return;
        }

        // Collider를 트리거로 설정
        zoneCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            Debug.Log("Enemy attempted to enter the restricted zone but was blocked.");
            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                enemyRb.AddForce(pushDirection * 5f, ForceMode2D.Impulse); // 적을 밀어내기
            }
        }
        else if (collision.CompareTag(playerTag))
        {
            Debug.Log("Player entered the restricted zone.");
            // 플레이어는 아무 제한 없이 통과 가능
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("Player exited the restricted zone.");
        }
        else if (collision.CompareTag(enemyTag))
        {
            Debug.Log("Enemy exited the restricted zone.");
        }
    }
}
