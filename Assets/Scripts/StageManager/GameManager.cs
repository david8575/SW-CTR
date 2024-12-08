using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    public int starsCollected { get; private set; } = 0;

    [SerializeField]
    private int enemieCount = 0;
    public int EnemieCount
    {
        get { return enemieCount; }
        set
        {
            Debug.Log("Enemy Count: " + value);
            enemieCount = value;
            if (enemieCount < 0)
                enemieCount = 0;

            if (CurrentStage != null)
                CurrentStage.SetEnemyCount();
        }
    }
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
            EnemieCount = 0;
            IsAllKill = false;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    public void CheckAllEnemiesDefeated()
    {
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        EnemieCount--;
        CurrentStage?.SetEnemyCount();

        if (EnemieCount <= 0 && IsAllKill == false)
        {
            Debug.Log("All enemies defeated!");
            IsAllKill = true;
            CollectStar();
        }
    }
}
