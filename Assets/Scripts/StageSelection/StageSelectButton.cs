using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    Button button;
    int index;
    StageSelectManager manager;

    public GameObject[] stars;
    public TextMeshProUGUI timeText;

    public void Init(StageSelectManager man, int idx)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);

        index = idx;
        manager = man;

        var data = DataManager.Instance.SaveData;
        var myData = data.Stages[index];

        if (index > 1)
        {
            if (data.Stages[index - 1].isClear)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }

        if (myData.isClear)
        {
            if (myData.isAllKill)
                stars[0].SetActive(true);
            if (myData.isPuzzleClear)
                stars[1].SetActive(true);
            if (myData.isTimeClear)
                stars[2].SetActive(true);

            timeText.gameObject.SetActive(true);

            if (myData.bestTime == 0)
                timeText.text = "00:00";
            else
                timeText.text = string.Format("{0:00}:{1:00}", (int)(myData.bestTime / 60), (int)(myData.bestTime % 60));
        }
    }

    void OnButtonClick()
    {
        manager.OnStageButton(index + 2);
    }
}
