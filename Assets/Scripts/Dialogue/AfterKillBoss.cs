using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class AfterKillBoss : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string characterName; 
        [TextArea()] public string[] sentences;
        public Sprite characterSprite; 
    }

    public GameObject dialogueCanvas; 
    public GameObject dialogueBox; 
    public TMP_Text dialogueText; 
    public TMP_Text nameText; 
    public GameObject nameBox; 
    public Image characterImage;

    public Dialogue circle1Dialogue; 
    public Dialogue circle2Dialogue; 
    public GameObject optionPanel; 
    public string option1SceneName = "Option1Scene"; 
    public string option2SceneName = "Option2Scene"; 

    private Queue<string> circle1Sentences; 
    private Queue<string> circle2Sentences; 
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool isCircle1Turn = true; 

    // Start is called before the first frame update
    void Start()
    {
        circle1Sentences = new Queue<string>();
        circle2Sentences = new Queue<string>();
        dialogueBox.SetActive(false);
        nameBox.SetActive(false);
        optionPanel.SetActive(false); 

        InitializeDialogues();
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextSentence();
        }
    }

    private void InitializeDialogues()
    {
        foreach (string sentence in circle1Dialogue.sentences)
        {
            circle1Sentences.Enqueue(sentence);
        }

        foreach (string sentence in circle2Dialogue.sentences)
        {
            circle2Sentences.Enqueue(sentence);
        }
    }

    public void StartDialogue()
    {
        dialogueBox.SetActive(true); 
        nameBox.SetActive(true); 
        isDialogueActive = true;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (circle1Sentences.Count == 0 && circle2Sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (isCircle1Turn)
        {
            if (circle1Sentences.Count > 0)
            {
                nameText.text = circle1Dialogue.characterName; 
                characterImage.sprite = circle1Dialogue.characterSprite;
                string sentence = circle1Sentences.Dequeue();
                if (string.IsNullOrWhiteSpace(sentence)) 
                {
                    isCircle1Turn = false; 
                    DisplayNextSentence(); 
                    return;
                }
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
            }
            else
            {
                isCircle1Turn = false;
                DisplayNextSentence();
            }
        }
        else
        {
            if (circle2Sentences.Count > 0)
            {
                nameText.text = circle2Dialogue.characterName; 
                characterImage.sprite = circle2Dialogue.characterSprite; 
                string sentence = circle2Sentences.Dequeue();
                if (string.IsNullOrWhiteSpace(sentence))
                {
                    isCircle1Turn = true; 
                    DisplayNextSentence(); 
                    return;
                }
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
            }
            else
            {
                isCircle1Turn = true; 
                DisplayNextSentence();
            }
        }

        isCircle1Turn = !isCircle1Turn;
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
        dialogueBox.SetActive(false); 
        nameBox.SetActive(false); 
        optionPanel.SetActive(true); 
        isDialogueActive = false; 
    }

    public void ChooseOption1()
    {
        SceneManager.LoadScene(option1SceneName); 
    }

    public void ChooseOption2()
    {
        SceneManager.LoadScene(option2SceneName); 
    }
}
