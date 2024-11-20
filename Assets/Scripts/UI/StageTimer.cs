using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StageTimer : MonoBehaviour
{
    public float time = 0;
    TextMeshProUGUI timeText;
    public bool timerOn = false;

    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        timerOn = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (timerOn)
        {
            time += Time.deltaTime;
            // 00(초):00(밀리초) 형식으로 표시
            timeText.text = string.Format("{0:00}:{1:00}", (int)time, (int)(time * 100) % 100);
        }
    }
}
