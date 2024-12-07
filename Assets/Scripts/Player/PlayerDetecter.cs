using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PlayerDetecter : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    bool isPlayerInside = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerInside && collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
            OnPlayerEnter?.Invoke();
        }
    }
}
