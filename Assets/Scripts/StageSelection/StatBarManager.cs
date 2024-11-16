using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarManager : MonoBehaviour
{
    // �ɷ�ġ �� �迭
    public StatBar[] statBars;

    // ���� �ɷ�ġ ǥ��
    public TextMeshProUGUI statPointText;

    // ����� ������
    GameData data;

    private void Start()
    {
        data = DataManager.Instance.SaveData;

        AddLeftStatPoint(0);

        // �ɷ�ġ �� ���� ����
        foreach (var statBar in statBars)
        {
            statBar.manager = this;
            switch (statBar.statName)
            {
                case "health":
                    statBar.SetStatPoint(data.healthStat);
                    break;
                case "attack":
                    statBar.SetStatPoint(data.attackStat);
                    break;
                case "defense":
                    statBar.SetStatPoint(data.defenseStat);
                    break;
                case "speed":
                    statBar.SetStatPoint(data.speedStat);
                    break;
                case "cooldown":
                    statBar.SetStatPoint(data.cooldownStat);
                    break;

            }
            statBar.UpdateButton();
        }

    }

    // ���� �ɷ�ġ �ٿ� ���߾� �ɷ�ġ �߰�
    public void AddStatPoint(string name, int value)
    {
        switch (name)
        {
            case "health":
                data.healthStat += value;
                break;
            case "attack":
                data.attackStat += value;
                break;
            case "defense":
                data.defenseStat += value;
                break;
            case "speed":
                data.speedStat += value;
                break;
            case "cooldown":
                data.cooldownStat += value;
                break;
        }

        AddLeftStatPoint(-value);

        // ���� ��������Ʈ�� 0�̸� ��� +��ư ��Ȱ��ȭ�� �ؾ��ϱ⿡ ��� ��ư�� ������Ʈ�� �ɾ���.
        foreach (var statBar in statBars)
        {
            statBar.UpdateButton();
        }
    }

    // ���� ����Ʈ �����ϱ�
    public void AddLeftStatPoint(int value)
    {
        data.leftStatPoint += value;
        statPointText.text = "���� ����Ʈ : " + data.leftStatPoint.ToString();
    }
}
