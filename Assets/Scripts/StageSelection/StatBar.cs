using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    // ���׷��̵� ����
    [SerializeField]
    int statPoint = 0;
    [SerializeField]
    int maxPoint = 10;

    // ���׷��̵� ������
    public Image progress;
    public StatBarManager manager;

    // ����
    public string statName;

    // �߰�, ���� ��ư
    public GameObject plusButton;
    public GameObject minusButton;

    // ���׷��̵� ���� �ؽ�Ʈ
    public TextMeshProUGUI statText;

    public void OnPlusButton()
    {
        AddStatPoint(1);
    }

    public void OnMinusButton()
    {
        AddStatPoint(-1);
    }

    // ��ư �� ���� ��Ȳ�̸� �Ⱥ��̰� (���׷��̵� ����Ʈ�� ���ų� �ִ�ġ�� ��)
    public void UpdateButton()
    {
        plusButton.SetActive(DataManager.Instance.SaveData.leftStatPoint > 0 && statPoint < maxPoint);
        minusButton.SetActive(statPoint > 0);
    }

    // �ɷ�ġ �����ְ� �Ŵ��� ���� ������Ʈ
    public void AddStatPoint(int value)
    {
        SetStatPoint(statPoint + value);

        manager.AddStatPoint(statName, value);
    }

    // �ɷ�ġ ����
    public void SetStatPoint(int value)
    {
        statPoint = value;

        if (statPoint < 0)
            statPoint = 0;
        else if (statPoint > maxPoint)
            statPoint = maxPoint;


        progress.fillAmount = (float)statPoint / maxPoint;
        statText.gameObject.SetActive(statPoint > 0);
        statText.text = string.Format("{0}/{1}", statPoint, maxPoint);
    }
}
