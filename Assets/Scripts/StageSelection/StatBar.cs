using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    // 업그레이드 정보
    [SerializeField]
    int statPoint = 0;
    [SerializeField]
    int maxPoint = 10;

    // 업그레이드 게이지
    public Image progress;
    public StatBarManager manager;

    // 정보
    public string statName;

    // 추가, 제거 버튼
    public GameObject plusButton;
    public GameObject minusButton;

    // 업그레이드 정보 텍스트
    public TextMeshProUGUI statText;

    public void OnPlusButton()
    {
        AddStatPoint(1);
    }

    public void OnMinusButton()
    {
        AddStatPoint(-1);
    }

    // 버튼 못 누를 상황이면 안보이게 (업그레이드 포인트가 없거나 최대치일 때)
    public void UpdateButton()
    {
        plusButton.SetActive(DataManager.Instance.SaveData.leftStatPoint > 0 && statPoint < maxPoint);
        minusButton.SetActive(statPoint > 0);
    }

    // 능력치 더해주고 매니저 통해 업데이트
    public void AddStatPoint(int value)
    {
        SetStatPoint(statPoint + value);

        manager.AddStatPoint(statName, value);
    }

    // 능력치 설정
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
