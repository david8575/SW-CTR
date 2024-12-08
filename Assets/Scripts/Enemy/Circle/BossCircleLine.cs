using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircleLine : MonoBehaviour
{
    public Transform[] line;
    public Transform movingTaget;

    public float radius = 1f; // ������

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

        // 1. �÷��̾� ��ġ ���
        Vector3 playerPos = PlayerController.Instance.GetShapeTransform().position;
        playerPos.z = 0;

        // 2. �÷��̾� ��ġ�� �������� �߽ɰ��� ���� ���
        Vector3 direction = playerPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x); // ���� ����

        // 20 ~ 160 ���̷� ����
        angle = Mathf.Clamp(Mathf.Abs(angle), Mathf.Deg2Rad * 20, Mathf.Deg2Rad * 160);

        // 3. �� ��輱 ���� ��ǥ ���
        float x = transform.position.x + Mathf.Cos(angle) * radius;
        float y = transform.position.y + Mathf.Sin(angle) * radius;

        // 4. ������Ʈ ��ġ ����
        movingTaget.position = new Vector3(x, y, movingTaget.position.z);
    }
}
