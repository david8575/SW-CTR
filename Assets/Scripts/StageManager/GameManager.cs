using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public int starsCollected { get; private set; } = 0;
    public int enemieCount { get; set; } = 0;
    public bool IsAllKill { get; private set; } = false;

    [SerializeField]
    StageBase stage;
    public StageBase CurrentStage
    {
        get { return stage; }
        set
        {
            stage = value;
            starsCollected = 0;
            enemieCount = 0;
            IsAllKill = false;
        }
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
        CurrentStage.SetStarCount(starsCollected);
    }

    public void CheckAllEnemiesDefeated()
    {
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemieCount--;
        
        if (enemieCount == 0 && IsAllKill == false)
        {
            Debug.Log("All enemies defeated!");
            IsAllKill = true;
            CollectStar();
        }
    }
}
