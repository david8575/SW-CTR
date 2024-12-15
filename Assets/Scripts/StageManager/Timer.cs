using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLimit = 60f; 
    private bool timerRunning = true;
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeLimit)
            {
                timerRunning = false;
                Debug.Log("Time's up!");
            }
        }
    }

    public bool IsCompletedWithinTime()
    {
        return elapsedTime <= timeLimit;
    }

    public void StopTimer()
    {
        timerRunning = false;
        Debug.Log("Timer stopped. Time elapsed: " + elapsedTime);
    }
}
