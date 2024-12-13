using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float checkInterval = 0.5f;

    private void Start()
    {
        InvokeRepeating(nameof(CheckEnemies), checkInterval, checkInterval);
    }

    private void CheckEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            SceneManager.LoadScene(14);
        }
    }
}
