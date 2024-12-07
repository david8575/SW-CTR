using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : StageBase
{
    [Header("TutorialManager")]
    public DialogueManager dialogueManager;
    public GameObject[] cameraArray;
    public NormalTriangle tutoTriangle;

    public GameObject[] canvasList;

    public GameObject[] tutoObjects;

    public GameObject[] tutoList;

    PlayerInputActions playerInputActions;

    bool mouseClick = false;
    bool jump = false;

    WaitForSeconds wait5 = new WaitForSeconds(0.5f);
    WaitForSecondsRealtime waitReal2 = new WaitForSecondsRealtime(0.2f);
    WaitWhile waitDialogue = new WaitWhile(() => DialogueManager.IsInDialogue);
    WaitWhile waitMouse;

    protected override void Awake()
    {
        waitMouse = new WaitWhile(() => !mouseClick);

        FadeManager.Instance.SetFade(true);

        cameraArray[0].SetActive(true);
        for (int i = 1; i < cameraArray.Length; i++)
        {
            cameraArray[i].SetActive(false);
        }

        for (int i = 0; i < canvasList.Length; i++)
        {
            canvasList[i].SetActive(false);
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.DialougeActions.NextDialouge.performed += _ => mouseClick = true;
        playerInputActions.PlayerActions.Jump.performed += _ => jump = true;

        foreach (var button in stageUI.finishButtons)
            button.onClick.AddListener(() => {
                StartCoroutine(OnFinishButton());
                    });

        StartCoroutine(StoryCoroutine());
    }

    void Update()
    {
        if (PlayerController.Instance.hp < 50)
        {
            PlayerController.Instance.hp = 50;
        }

    }
    IEnumerator StoryCoroutine()
    {
        // -----------------------------------------------------------------
        // 인트로

        yield return wait5;
        yield return wait5;

        dialogueManager.StartSpecialDialogue("intro", 0.07f);
        yield return waitDialogue;
        yield return wait5;

        cameraArray[0].SetActive(false);
        cameraArray[1].SetActive(true);

        yield return FadeManager.Instance.FadeOut();
        // -----------------------------------------------------------------
        // 원 첫번째 대화 및 점프 튜토리얼
        #region firstchat

        yield return wait5;

        dialogueManager.StartDialogue("first_chat");

        yield return waitDialogue;

        tutoTriangle.EnemyEnabled = true;
        yield return new WaitWhile(() => tutoTriangle.IsAttacking == false);

        Time.timeScale = 0;
        canvasList[0].SetActive(true);
        canvasList[1].SetActive(true);
        yield return waitReal2;

        mouseClick = false;
        playerInputActions.Enable();
        yield return waitMouse;

        canvasList[1].SetActive(false);
        canvasList[2].SetActive(true);
        mouseClick = false;
        yield return waitReal2;
        mouseClick = false;
        yield return waitMouse;

        canvasList[2].SetActive(false);
        canvasList[3].SetActive(true);
        mouseClick = false;
        yield return waitReal2;

        PlayerController.Instance.SetInputSystem(true);
        PlayerController.Instance.canSpecial = false;
        jump = false;
        yield return new WaitWhile(() => !jump);

        canvasList[3].SetActive(false);
        canvasList[0].SetActive(false);

        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);

        tutoTriangle.EnemyEnabled = false;
        dialogueManager.StartDialogue("after_jump");
        yield return waitDialogue;
        yield return wait5;
        yield return wait5;
        #endregion

        // -----------------------------------------------------------------
        // 삼각형으로부터 도망가기

        Time.timeScale = 0;
        playerInputActions.Enable();
        PlayerController.Instance.SetInputSystem(false);
        canvasList[4].SetActive(true);
        canvasList[5].SetActive(true);

        PlayerController.Instance.uiManager.cooldownCircle.gameObject.SetActive(true);
        PlayerController.Instance.ResetCooltime();
        for (int i = 0; i < 3; i++)
        {
            yield return waitReal2;
            mouseClick = false;
            yield return waitMouse;

            canvasList[5 + i].SetActive(false);
            canvasList[6 + i].SetActive(true);
        }
        yield return waitReal2;
        mouseClick = false;
        yield return waitMouse;

        canvasList[4].SetActive(false);
        Time.timeScale = 1;
        PlayerController.Instance.SetInputSystem(true);
        tutoTriangle.EnemyEnabled = true;
        playerInputActions.Disable();
    }

    public void DetectPlayer(int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(SecondCoroutine());
                break;
            case 1:
                StartCoroutine(ThirdCoroutine());
                break;
            case 2:
                StartCoroutine(GetTriangle());
                break;
            case 3:
                dialogueManager.StartDialogue("after_triangle", 0.05f, () => PlayerController.Instance.SetInputSystem(true));
                break;
            case 4:
                tutoObjects[1].SetActive(false);
                StartCoroutine(TutoMsg(tutoList[0]));
                break;
            case 5:
                GameManager.Instance.EnemieCount = 0;
                GameManager.Instance.CheckAllEnemiesDefeated();
                StartCoroutine(TutoMsg(tutoList[1]));
                break;
        }
    }

    void ResetPlayerMovement()
    {
        PlayerController.Instance.rb.velocity = Vector2.zero;
        PlayerController.Instance.rb.angularVelocity = 0;
        PlayerController.Instance.rb.totalForce = Vector2.zero;
        PlayerController.Instance.SetInputSystem(false);
    }

    IEnumerator SecondCoroutine()
    {
        ResetPlayerMovement();

        dialogueManager.StartDialogue("scene2_");
        yield return waitDialogue;
        PlayerController.Instance.SetInputSystem(true);
    }

    IEnumerator ThirdCoroutine()
    {
        ResetPlayerMovement();

        cameraArray[1].SetActive(false);
        cameraArray[2].SetActive(true);
        yield return new WaitForSeconds(1f);

        dialogueManager.StartDialogue("see_deadbody");
        yield return waitDialogue;

        cameraArray[2].SetActive(false);
        cameraArray[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        PlayerController.Instance.SetInputSystem(true);
    }

    IEnumerator GetTriangle()
    {
        ResetPlayerMovement();

        var wait = new WaitForSeconds(0.05f);
        SpriteRenderer sp = tutoObjects[0].GetComponent<SpriteRenderer>();
        Transform tr = tutoObjects[0].transform;
        Color c = sp.color;
        float alpha = 0.6f;
        float lerp = 0.05f;
        while (alpha > 0)
        {
            alpha -= 0.01f;
            c.a = alpha;
            sp.color = c;
            tr.position = Vector3.Lerp(tr.position, PlayerController.Instance.GetShapeTransform().position, lerp);

            yield return wait;
        }
        dialogueManager.StartDialogue("get_triangle");
        yield return waitDialogue;

        canvasList[9].SetActive(true);
        PlayerController.Instance.uiManager.disableShape(1, false);
        PlayerController.Instance.CanChangeShape[1] = true;
        canvasList[10].SetActive(true);
        canvasList[11].SetActive(true);

        playerInputActions.Enable();
        mouseClick = false;
        yield return waitMouse;

        canvasList[11].SetActive(false);
        canvasList[12].SetActive(true);
        mouseClick = false;
        yield return waitMouse;

        canvasList[12].SetActive(false);
        canvasList[13].SetActive(true);
        mouseClick = false;
        yield return waitMouse;

        canvasList[10].SetActive(false);
        canvasList[13].SetActive(false);
        PlayerController.Instance.SetInputSystem(true);

        yield return new WaitWhile(() => PlayerController.Instance.ShapeInfo is Circle);

        PlayerController.Instance.SetInputSystem(false);
        canvasList[10].SetActive(true);
        canvasList[14].SetActive(true);
        mouseClick = false;
        yield return waitMouse;

        canvasList[14].SetActive(false);
        canvasList[15].SetActive(true);
        mouseClick = false;
        yield return waitMouse;

        canvasList[10].SetActive(false);
        playerInputActions.Disable();
        PlayerController.Instance.ResetCooltime();
        PlayerController.Instance.SetInputSystem(true);
    }

    IEnumerator TutoMsg(GameObject tuto)
    {
        int cnt = tuto.transform.childCount;
        GameObject[] objects = new GameObject[cnt];
        for (int i = 0; i < cnt; i++)
        {
            objects[i] = tuto.transform.GetChild(i).gameObject;
        }

        PlayerController.Instance.SetInputSystem(false);
        playerInputActions.Enable();
        objects[0].SetActive(true);
        for (int i = 1; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
            mouseClick = false;
            yield return waitMouse;
            objects[i].SetActive(false);
        }
        objects[0].SetActive(false);
        playerInputActions.Dispose();
        PlayerController.Instance.SetInputSystem(true);
    }

#if UNITY_EDITOR
    [ContextMenu("SkipTutorial")]
    public void SkipTutorial()
    {
        StartCoroutine(OnFinishButton());
    }
#endif
}
