using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public StageTimer stageTimer;
    public TextMeshProUGUI starText;

    public GameObject[] clearObjects;
    public GameObject[] stars;
    public TextMeshProUGUI[] timeTexts;
    public TextMeshProUGUI puzzleText;

    public GameObject[] GameoverObjects;
    public TextMeshProUGUI GameoverTimeText;

    public Button[] finishButtons;

    private void Start()
    {
        for (int i = 0; i < clearObjects.Length; i++)
        {
            if (i < stars.Length)
                stars[i].SetActive(false);
            clearObjects[i].SetActive(false);
        }

        for (int i = 0; i < GameoverObjects.Length; i++)
        {
            GameoverObjects[i].SetActive(false);
        }

    }

    public IEnumerator StageCorutine()
    {
        var wait = new WaitForSeconds(1f);
        int i;

        for (i = 0; i < 4; i++)
        {
            clearObjects[i].SetActive(true);
            yield return wait;
        }

        var saveData = DataManager.Instance.SaveData;
        var stageInfo = saveData.Stages[GameManager.instance.CurrentStage.stageNumber];

        if (stageInfo.isAllKill)
        {
            stars[0].SetActive(true);
            yield return wait;
        }

        if (stageInfo.isPuzzleClear)
        {
            stars[1].SetActive(true);
            yield return wait;
        }

        if (stageInfo.isTimeClear)
        {
            stars[2].SetActive(true);
            yield return wait;
        }

        clearObjects[i].SetActive(true);
    }

    public IEnumerator GameoverCorutine()
    {
        var wait = new WaitForSeconds(1f);

        for (int i = 0; i < GameoverObjects.Length; i++)
        {
            GameoverObjects[i].SetActive(true);
            yield return wait;
        }
    }
}
