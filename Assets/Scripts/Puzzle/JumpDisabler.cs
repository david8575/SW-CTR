using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpDisabler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        // PlayerController가 있는 객체(혹은 부모 객체)를 찾음
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.canJump = false; // 점프 비활성화
            Debug.Log("Jump disabled for the player.");
        }
        else
        {
            Debug.LogError("PlayerController script not found on the player object or its parents!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // PlayerController가 있는 객체(혹은 부모 객체)를 찾음
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.canJump = true; // 점프 활성화
            Debug.Log("Jump enabled for the player.");
        }
    }
}
