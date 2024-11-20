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

    void Start()
    {
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

    public void OnStageButton(int id)
    {
        if (id == 3)
        {
            SceneManager.LoadScene("Stage3_presentation");
        }
    }

    // �ɷ�ġ ��ư �ؿ� ���� ����Ʈ ������ �˸��� ǥ��
    void UpdateStatAlarm() => statAlarmText.gameObject.SetActive(data.leftStatPoint > 0);
}
