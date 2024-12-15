using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static bool IsInDialogue = false;

    PlayerInputActions inputActions;

    [Header("Dialogue UI")]
    public TextMeshProUGUI dialogueText;
    public Image dialogueSpeakerImage;
    public TextMeshProUGUI dialogueSpeakerNameText;
    public Animator dialougePanel;

    [Header("Dialogue File")]
    public string dialogueFileName;
    public Dictionary<string ,Dictionary<string, string>> dialouge;

    [Header("Special Dialogue")]
    public TextMeshProUGUI specialDialogueText;

    Queue<Dictionary<string, string>> chat = new Queue<Dictionary<string, string>>();

    bool isClicked = false;

    Dictionary<string, string> speakerInfo = new Dictionary<string, string>()
    {
        {"", "" },
        {"player", "���ΰ�" },
        {"actriangle", "�ﰢ��" },
    };

    public bool playerAutoEnable = true;

    private void Start()
    {
        inputActions = new PlayerInputActions();

        inputActions.DialougeActions.NextDialouge.performed += NextDialouge;

        // dialougeFileName�� �ش��ϴ� CSV ������ �о dialouge ��ųʸ��� ����
        dialouge = CSVReader.Read("Dialogue/" + dialogueFileName);
        
    }

    public void StartDialogue(string key, float interval = 0.05f, Action callBack = null)
    {
        SetDialogue(key);

        dialougePanel.SetBool("show", true);
        StartCoroutine(DialogueCoroutine(dialogueText, interval, callBack));
    }

    public void StartSpecialDialogue(string key, float interval = 0.05f, Action callBack = null)
    {
        SetDialogue(key);
        specialDialogueText.gameObject.SetActive(true);
        StartCoroutine(DialogueCoroutine(specialDialogueText, interval, callBack));
    }

    IEnumerator DialogueCoroutine(TextMeshProUGUI curDialougueText, float wait, Action callBack)
    {
        WaitWhile waitClick = new WaitWhile(() => !isClicked);
        WaitForSeconds waitTime = new WaitForSeconds(wait);

        yield return waitTime;

        while (chat.Count > 0)
        {
            var curChat = chat.Dequeue();

            string words = curChat["chat"];
            string output = "";

            //Debug.Log(words);

            dialogueSpeakerNameText.text = speakerInfo[curChat["speaker"]];
            if (curChat["speaker"] != "")
            {
                dialogueSpeakerImage.color = new Color(1, 1, 1, 1);
                dialogueSpeakerImage.sprite = Resources.Load<Sprite>("Dialogue/sprite/" + curChat["speaker"]);
            }
            else
            {
                dialogueSpeakerImage.color = new Color(0, 0, 0, 0);
            }

            for (int i = 0; i < words.Length; i++)
            {
                curDialougueText.text = output;
                output += words[i];
                yield return waitTime;

                if (isClicked)
                {
                    isClicked = false;
                    break;
                }
            }
            curDialougueText.text = words;

            yield return waitClick;
            isClicked = false;
        }

        EndDialouge();
        callBack?.Invoke();
    }

    void EndDialouge()
    {

        inputActions.DialougeActions.Disable();

        dialougePanel.SetBool("show", false);
        if (specialDialogueText != null)
            specialDialogueText.gameObject.SetActive(false);

        IsInDialogue = false;

        if (PlayerController.Instance != null && playerAutoEnable)
            PlayerController.Instance.SetInputSystem(true);
    }

    private void NextDialouge(InputAction.CallbackContext context)
    {
        isClicked = true;
    }

    void SetDialogue(string key)
    {
        IsInDialogue = true;

        if (PlayerController.Instance != null)
            PlayerController.Instance.SetInputSystem(false);


        // key�� �ش��ϴ� ��ȭ�� chat ť�� ����
        int idx = 1;
        var dia = dialouge[key];
        chat.Clear();
        chat.Enqueue(dia);

        while (true)
        {
            if (dia["special"] == "end")
                break;

            // special�� end�� ���� ������ ��ȭ�� chat ť�� ����
            dia = dialouge[key + idx.ToString()];
            chat.Enqueue(dia);
            idx++;


        }

        inputActions.DialougeActions.Enable();
    }
}
