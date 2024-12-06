using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private void Start()
    {
        inputActions = new PlayerInputActions();

        inputActions.DialougeActions.NextDialouge.performed += NextDialouge;

        // dialougeFileName�� �ش��ϴ� CSV ������ �о dialouge ��ųʸ��� ����
        dialouge = CSVReader.Read("Dialogue/" + dialogueFileName);
        
    }

    public void StartDialouge(string key, float interval = 0.05f, bool UseSpecialText =false)
    {
        IsInDialogue = true;

        if (PlayerController.Instance != null)
            PlayerController.Instance.SetInputSystem(false);


        // key�� �ش��ϴ� ��ȭ�� chat ť�� ����
        int idx = 1;
        chat.Clear();
        chat.Enqueue(dialouge[key]);

        while (true)
        {
            // special�� end�� ���� ������ ��ȭ�� chat ť�� ����
            var dia = dialouge[key + idx.ToString()];
            chat.Enqueue(dia);
            idx++;

            if (dia["special"] == "end")
                break;
        }

        inputActions.DialougeActions.Enable();

        dialougePanel.SetBool("show", true);
        if (UseSpecialText)
        {
            specialDialogueText.gameObject.SetActive(true);
            StartCoroutine(DialogueCoroutine(specialDialogueText, interval));
        }
        else
        {
            StartCoroutine(DialogueCoroutine(dialogueText, interval));
        }
    }

    IEnumerator DialogueCoroutine(TextMeshProUGUI curDialougueText, float wait)
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
    }

    void EndDialouge()
    {

        inputActions.DialougeActions.Disable();

        dialougePanel.SetBool("show", false);
        if (specialDialogueText != null)
            specialDialogueText.gameObject.SetActive(false);

        IsInDialogue = false;

        if (PlayerController.Instance != null)
            PlayerController.Instance.SetInputSystem(true);
    }

    private void NextDialouge(InputAction.CallbackContext context)
    {
        isClicked = true;
    }
}
