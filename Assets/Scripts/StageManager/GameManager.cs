using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    private int starsCollected = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectStar()
    {
        starsCollected++;
        Debug.Log("Star Collected! Total Stars: " + starsCollected);
    }

    public int GetStarsCollected()
    {
        return starsCollected;
    }

    public void CheckAllEnemiesDefeated()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Debug.Log("All enemies defeated! Activating Clear Point.");
            ActivateClearPoint();
        }
    }

    private void ActivateClearPoint()
    {
        GameObject clearPoint = GameObject.Find("ClearPoint");
        if (clearPoint != null)
        {
            clearPoint.SetActive(true);
        }
    }
}
