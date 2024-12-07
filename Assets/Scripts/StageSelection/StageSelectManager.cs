using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [Header ("SaveFileViewer")]
    public GameData data;

    [Header("StatPanel")]
    public GameObject statPanel;
    public TextMeshProUGUI statAlarmText;

    [Header("Other")]
    public Image fade;
    public GameObject buttonHighestParents;

    void Start()
    {
        FadeManager.Instance.FadeOut();
        data = DataManager.Instance.SaveData;
        UpdateStatAlarm();

        int idx = 0;
        for (int i = 0; i < buttonHighestParents.transform.childCount; i++)
        {
            var buttonParent = buttonHighestParents.transform.GetChild(i);
            for (int j = 0; j < buttonParent.childCount; j++)
            {
                buttonParent.GetChild(j).GetComponent<StageSelectButton>().Init(this, idx++);

            }
        }
    }

    // 데이터 초기화용 디버그 기능
    [ContextMenu("ResetData")]
    public void ResetData()
    {
        DataManager.Instance.RemoveGameData();
        data = DataManager.Instance.SaveData;
    }


    public void OnStatButton()
    {
        statPanel.SetActive(true);
    }

    public void OnStatExitButton()
    {
        statPanel.SetActive(false);
        DataManager.Instance.SaveGameData();
        UpdateStatAlarm();
    }

    public void OnStageButton(int sceneIdx)
    {
        StartCoroutine(StageCutscene(sceneIdx));
    }

    IEnumerator StageCutscene(int sceneIdx)
    {
        var load = SceneManager.LoadSceneAsync(sceneIdx);
        load.allowSceneActivation = false;

        yield return FadeManager.Instance.FadeIn();

        // 화면 전환
        load.allowSceneActivation = true;
    }


        // 능력치 버튼 밑에 남은 포인트 있음을 알리는 표시
    void UpdateStatAlarm() => statAlarmText.gameObject.SetActive(data.leftStatPoint > 0);
}
