using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public float maxScale = 1.5f;

    public bool IsForPlayer = false;

    public void Appear(Vector3 pos)
    {
        transform.position = pos;

        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        // 점점 커지는 애니메이션
        float scale = 0.1f;
        while (scale < maxScale)
        {
            scale *= 2;
            transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        // 점점 작아지는 애니메이션
        while (scale > 0.1f)
        {
            scale /= 2;
            transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ShockWave " + collision.name);
        if (!IsForPlayer)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (PlayerController.Instance.IsInvincible == true)
                    return;

                PlayerController.Instance.TakeDamage(10);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("ShockWave OnTriggerEnter2D Enemy");
                var enemy = collision.gameObject.GetComponent<EnemyBase>();
                enemy.TakeDamage(PlayerController.Instance.attack);
            }
        }

    }
}
