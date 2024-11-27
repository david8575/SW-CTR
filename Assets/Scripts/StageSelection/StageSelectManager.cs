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

    void Start()
    {
        FadeManager.Instance.FadeOut();
        data = DataManager.Instance.SaveData;
        UpdateStatAlarm();
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

    public void OnStageButton(string sceneName)
    {
        if (sceneName != "")
        {
            StartCoroutine(StageCutscene(sceneName));
        }
    }

    IEnumerator StageCutscene(string sceneName)
    {
        var load = SceneManager.LoadSceneAsync(sceneName);
        load.allowSceneActivation = false;

        yield return FadeManager.Instance.FadeIn();

        // ȭ�� ��ȯ
        load.allowSceneActivation = true;
    }


        // �ɷ�ġ ��ư �ؿ� ���� ����Ʈ ������ �˸��� ǥ��
    void UpdateStatAlarm() => statAlarmText.gameObject.SetActive(data.leftStatPoint > 0);
}
