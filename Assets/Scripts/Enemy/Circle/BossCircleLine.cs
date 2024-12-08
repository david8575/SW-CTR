using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircleLine : MonoBehaviour
{
    public Transform[] line;
    public Transform movingTaget;

    public float radius = 1f; // 반지름

    private LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = line.Length;
    }

    private void Update()
    {
        for (int i = 0; i < line.Length; i++)
        {
            lineRenderer.SetPosition(i, line[i].position);
        }

        // 1. 플레이어 위치 계산
        Vector3 playerPos = PlayerController.Instance.GetShapeTransform().position;
        playerPos.z = 0;

        // 2. 플레이어 위치를 기준으로 중심과의 각도 계산
        Vector3 direction = playerPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x); // 라디안 단위

        // 20 ~ 160 사이로 제한
        angle = Mathf.Clamp(Mathf.Abs(angle), Mathf.Deg2Rad * 20, Mathf.Deg2Rad * 160);

        // 3. 원 경계선 상의 좌표 계산
        float x = transform.position.x + Mathf.Cos(angle) * radius;
        float y = transform.position.y + Mathf.Sin(angle) * radius;

        // 4. 오브젝트 위치 갱신
        movingTaget.position = new Vector3(x, y, movingTaget.position.z);
    }
}
