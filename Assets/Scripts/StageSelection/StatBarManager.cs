using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarManager : MonoBehaviour
{
    // 능력치 바 배열
    public StatBar[] statBars;

    // 남으 능력치 표시
    public TextMeshProUGUI statPointText;

    // 저장된 데이터
    GameData data;

    private void Start()
    {
        data = DataManager.Instance.SaveData;

        AddLeftStatPoint(0);

        // 능력치 바 최초 세팅
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

    // 들어온 능력치 바에 맞추어 능력치 추가
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

        // 남은 스탯포인트가 0이면 모든 +버튼 비활성화를 해야하기에 모든 버튼에 업데이트를 걸어줌.
        foreach (var statBar in statBars)
        {
            statBar.UpdateButton();
        }
    }

    // 남은 포인트 변경하기
    public void AddLeftStatPoint(int value)
    {
        data.leftStatPoint += value;
        statPointText.text = "남은 포인트 : " + data.leftStatPoint.ToString();
    }
}
