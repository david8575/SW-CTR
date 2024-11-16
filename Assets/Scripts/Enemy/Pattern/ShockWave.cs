using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    

    void Start()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        // ���� Ŀ���� �ִϸ��̼�
        float scale = 0.1f;
        while (scale < 1.5f)
        {
            scale *= 2;
            transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        // ���� �۾����� �ִϸ��̼�
        while (scale > 0.1f)
        {
            scale /= 2;
            transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerController.Instance.IsInvincible == true)
                return;

            PlayerController.Instance.TakeDamage(10);
        }
    }
}
