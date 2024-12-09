using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpPad : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 15f; // 점프대에서 가할 힘의 크기

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 객체가 "Player" 태그를 가졌는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // 기존 속도를 초기화하고 위쪽 방향으로 힘을 가함
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0f); // 기존 Y 속도 제거
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}