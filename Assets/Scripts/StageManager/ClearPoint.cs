using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPoint : MonoBehaviour
{
    public bool IsPlayerInside { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInside = false;
        }
    }
}
