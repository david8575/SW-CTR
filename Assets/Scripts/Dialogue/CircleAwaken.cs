using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CircleAwaken : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string characterName;
        [TextArea()] public string[] sentences;
    }

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    public GameObject nameBox;
    public GameObject additionalCanvas; 

    public Dialogue dialogue;

    private Queue<string> sentences;
    private bool isDialogueActive = false;
    private bool isTyping = false;

    private GameObject player; 

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);

        player = GameObject.FindWithTag("Player");

        StartDialogue();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.characterName;
        sentences.Clear();

        if (player != null)
        {
            player.SetActive(false); 
        }

        if (additionalCanvas != null)
        {
            additionalCanvas.SetActive(false);
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        isDialogueActive = true;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueText.text = "";
        nameText.text = "";
        dialoguePanel.SetActive(false);
        nameBox.SetActive(false);

        if (player != null)
        {
            player.SetActive(true); 
        }

        if (additionalCanvas != null)
        {
            additionalCanvas.SetActive(true); 
        }

        isDialogueActive = false; 
    }
}
