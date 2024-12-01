using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    PlayerInputActions inputActions;

    [Header("Dialogue UI")]
    public TextMeshProUGUI dialogueText;
    public Image dialogueSpeakerImage;
    public TextMeshProUGUI dialogueSpeakerNameText;
    public Animator dialougePanel;

    [Header("Dialogue File")]
    public string dialogueFileName;
    public Dictionary<string ,Dictionary<string, string>> dialouge;

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

    [ContextMenu("test")]
    public void testScript() => StartDialouge("test");

    public void StartDialouge(string key)
    {
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
        StartCoroutine(DialogueCoroutine());
    }

    IEnumerator DialogueCoroutine()
    {
        WaitWhile waitClick = new WaitWhile(() => !isClicked);
        WaitForSeconds waitTime = new WaitForSeconds(0.05f);

        dialougePanel.SetBool("show", true);
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
                dialogueText.text = output;
                output += words[i];
                yield return waitTime;

                if (isClicked)
                {
                    isClicked = false;
                    break;
                }
            }
            dialogueText.text = words;

            yield return waitClick;
            isClicked = false;
        }

        EndDialouge();
    }

    public void EndDialouge()
    {
        if (PlayerController.Instance != null)
            PlayerController.Instance.SetInputSystem(true);
        inputActions.DialougeActions.Disable();

        dialougePanel.SetBool("show", false);
    }

    private void NextDialouge(InputAction.CallbackContext context)
    {
        isClicked = true;
    }
}
