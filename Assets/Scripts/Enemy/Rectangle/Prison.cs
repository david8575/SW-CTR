using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour
{
    Transform wall;
    public Transform[] points;


    void Start()
    {
        wall = transform.GetChild(0);
        StartCoroutine(PrisonCoroutine());
    }

    public IEnumerator PrisonCoroutine()
    {
        float time = 0;
        while (time < 0.5f)
        {
            wall.localScale = Vector3.Lerp(wall.localScale, Vector3.one, 0.1f);
            time += Time.deltaTime;
            yield return null;
        }
    }

}
