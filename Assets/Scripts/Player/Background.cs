using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private void Update()
    {
        if (PlayerController.Instance != null)
        {
            transform.position = PlayerController.Instance.GetShapeTransform().position;
        }
    }
}
