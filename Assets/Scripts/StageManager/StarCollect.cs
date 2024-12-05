using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Star collected");
            GameManager.Instance.CollectStar();
            GameManager.Instance.CurrentStage.puzzleClear = true;
            Destroy(gameObject);
        }
    }
}
