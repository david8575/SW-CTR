using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Circle : Shape
{
    public bool isCharging = false;

    [SerializeField]
    bool IsTrueCircle = false;

    public PlayerStatus TrueCircleStatus;

    float power;

    public Transform FireEffect;
    public CircleCollider2D attackCollider;

    public PhysicsMaterial2D bounceMaterial;
    public PhysicsMaterial2D defaultMaterial;

    public string ChargingSound;
    public string AttackSound;

    private void Start()
    {
        IsTrueCircle = DataManager.Instance.SaveData.UnlockTrueCircle;
        if (IsTrueCircle)
            status = TrueCircleStatus;
    }

    public override void OnSpecialStarted()
    {
        AudioManager.PlaySound(SpecialSound);
        // Ư���ɷ�Ű ������ ��¡ ����
        isCharging = true;
        power = 0.1f;
    }

    private void FixedUpdate()
    {
        // ��¡ �߿��� ���� ����ؼ� ������
        if (isCharging)
        {
            if (power < specialPower)
            {         
                power *= 2;
                float amount = power / specialPower;
                spriteRenderer.color = new Color(1f - amount, 1f - amount, 1f - amount, 1);
            }

            // Ư���ɷ� ������ �ִ�ġ
            if (power > specialPower)
            {
                power = specialPower;
                spriteRenderer.color = Color.red;
            }
        }
    }

    public override void OnSpecialCanceled()
    {
        if (controller.canSpecial == false || isCharging == false)
            return;
        isCharging = false;
        spriteRenderer.color = Color.white;

        
        

        //rb.velocity = Vector2.zero;

        // ���콺�� ���� ���콺 �������� ���� ����ŭ �߻�
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;

        Vector3 dir = (mousePos - transform.position).normalized;
        float dot = Vector3.Dot(dir, rb.velocity);
        if (dot < 0)
            rb.velocity = Vector2.zero;
        rb.AddForce(dir * power, ForceMode2D.Impulse);

        if (IsTrueCircle)
        {
            AudioManager.PlaySound(AttackSound);
            attackCoroutine = StartCoroutine(TrueCircleAttack());
        }
        else
        {
            AudioManager.PlaySound(ChargingSound);
        }
    }

    private void Update()
    {
        if (controller.isAttacking)
        {
            Vector3 dir = rb.velocity.normalized;
            // ������ 0��, ���� �̵� ���϶� 180��
            FireEffect.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        }
    }

    protected override void StopAttack()
    {
        FireEffect.gameObject.SetActive(false);
        rb.sharedMaterial = defaultMaterial;
        base.StopAttack();
    }

    IEnumerator TrueCircleAttack()
    {
        rb.sharedMaterial = bounceMaterial;
        FireEffect.gameObject.SetActive(true);
        controller.isAttacking = true;
        spriteRenderer.color = Color.red;
        attackCollider.enabled = true;

        yield return new WaitWhile(() => rb.velocity.magnitude > 4f);
        rb.sharedMaterial = defaultMaterial;
        spriteRenderer.color = color;
        controller.isAttacking = false;
        FireEffect.gameObject.SetActive(false);
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

            if (controller.isAttacking)
            {
                enemy.TakeDamage(attack);
            }
        }
    }
}
