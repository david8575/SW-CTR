using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint"); 
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
