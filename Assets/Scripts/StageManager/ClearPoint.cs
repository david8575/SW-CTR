using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearPoint : MonoBehaviour
{
    private bool isCleared = false;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCleared)
        {
            isCleared = true; 
            Debug.Log("Stage Cleared");

            GameManager.instance.CollectStar();
            GameManager.instance.StageClear();
        }
    }
}
