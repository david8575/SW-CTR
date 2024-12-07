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

    // ������ �ʱ�ȭ�� ����� ���
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

        // ȭ�� ��ȯ
        load.allowSceneActivation = true;
    }


        // �ɷ�ġ ��ư �ؿ� ���� ����Ʈ ������ �˸��� ǥ��
    void UpdateStatAlarm() => statAlarmText.gameObject.SetActive(data.leftStatPoint > 0);
}
