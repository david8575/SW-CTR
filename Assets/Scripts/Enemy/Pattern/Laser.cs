using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool isLaserAttack = true;
    public float damage = 1;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    void OnTriggerEnter2D(Collider2D collision)
    { 
        Debug.Log("Laser Collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isLaserAttack)
            {
                PlayerController.Instance.TakeDamage(damage);
            }
        }
    }
}
