using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (Instance == null)
            {
                var obj = Resources.Load<GameManager>("GameManager");
                instance = Instantiate(obj);
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

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

    public AudioManager audioManager;

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
