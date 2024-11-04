using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    // ������ �ɷ�ġ
    public float speed;
    public float maxSpeed;
    public float jumpForce;
    public float attack;
    public float defense;
    public float cooldown;
    public float specialPower;

    // �̵� ���� ������Ʈ
    public Rigidbody2D rb;
    protected PlayerController controller;

    // �ʱ�ȭ
    public void Init(PlayerController con)
    {
        rb = GetComponent<Rigidbody2D>();
        controller = con;
    }

    // Ư�� �ɷ� �߻� �Լ�
    public abstract void OnSpecialStarted();

    public abstract void OnSpecialCanceled();

    // �� ���˽� ���� ����
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.transform.position.y < transform.position.y)
            {
                controller.canJump = true;
            }
        }
    }
}
