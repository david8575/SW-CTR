using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageBase : MonoBehaviour
{
    [Header("Stage Base")]
    [SerializeField]
    protected ClearPoint clearPoint;
    public ClearPoint ClearPoint { get { return clearPoint; } }

    public float clearTime = 60f;
    public bool puzzleClear = false;
    public bool isBoss = false;

    public StageUI stageUI;

    public StageTimer StageTimer { get { return stageUI.stageTimer; } }
    public TextMeshProUGUI StarText { get { return stageUI.starText; } }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.instance.CurrentStage = this;
        clearPoint.gameObject.SetActive(false);

        stageUI.finishButton.onClick.AddListener(() => StartCoroutine(OnFinishButton()));

        FadeManager.Instance.FadeOut();
    }

    public void SetStarCount(int cnt)
    {
        StarText.text = "Stars: " + cnt.ToString();
    }

    public void EnableClearPoint()
    {
        clearPoint.gameObject.SetActive(true);
    }

    public void StageClear()
    {
        PlayerController.Instance.SetInputSystem(false);

        StageTimer.timerOn = false;

        if (StageTimer.time < clearTime)
            GameManager.instance.CollectStar();

        stageUI.timeTexts[0].text = string.Format("{0:00}:{1:00}", (int)StageTimer.time, (int)(StageTimer.time * 100) % 100);
        stageUI.timeTexts[1].text = string.Format("{0:00}:{1:00}", (int)clearTime, (int)(clearTime * 100) % 100);

        if (isBoss)
        {
            GameManager.instance.CollectStar();
            stageUI.puzzleText.text = "Boss Clear"; 
        }
        else
            stageUI.puzzleText.text = "Puzzle " + (puzzleClear ? "Clear" : "Fail");

        StartCoroutine(stageUI.StageCorutine());
    }

    IEnumerator OnFinishButton()
    {
        var scene = SceneManager.LoadSceneAsync("StageSelect");
        scene.allowSceneActivation = false;
        yield return FadeManager.Instance.FadeIn();

        scene.allowSceneActivation = true;
    }
}
