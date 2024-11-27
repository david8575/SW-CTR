using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public int starsCollected { get; private set; } = 0;
    int enemieCount = 0;

    StageBase stage;
    public StageBase CurrentStage
    {
        get { return stage; }
        set
        {
            stage = value;
            starsCollected = 0;
            enemieCount = 0;
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
    public void AddEnmey()
    {
        enemieCount++;
    }

    public void CollectStar()
    {
        starsCollected++;
        Debug.Log("Star Collected! Total Stars: " + starsCollected);
        CurrentStage.SetStarCount(starsCollected);
    }

    public void StageClear()
    {
        CurrentStage.StageClear();
    }

    public void CheckAllEnemiesDefeated()
    {
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemieCount--;
        if (enemieCount == 0)
        {
            Debug.Log("All enemies defeated! Activating Clear Point.");
            CurrentStage.EnableClearPoint();
        }
    }
}
