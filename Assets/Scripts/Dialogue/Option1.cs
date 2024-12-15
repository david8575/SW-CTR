using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class Option1 : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string characterName; 
        [TextArea()] public string[] sentences; 
        public Sprite characterSprite; 
    }

    public GameObject fadePanel; // 검은 화면용 패널
    public TMP_Text fadeText; // 텍스트 표시용
    public GameObject dialogueCanvas; 
    public GameObject dialogueBox; 
    public TMP_Text dialogueText; 
    public TMP_Text nameText; 
    public GameObject nameBox; 
    public Image characterImage; 

    public Dialogue circle1Dialogue; 
    public Dialogue circle2Dialogue; 
    public GameObject optionPanel; 

    private Queue<string> circle1Sentences; 
    private Queue<string> circle2Sentences; 
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool isCircle1Turn = true; 

    void Start()
    {
        circle1Sentences = new Queue<string>();
        circle2Sentences = new Queue<string>();
        dialogueBox.SetActive(false);
        nameBox.SetActive(false);
        fadePanel.SetActive(true); // 처음 검은 화면 활성화
        fadeText.text = ""; 

        StartCoroutine(StartScene());
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextSentence();
        }
    }

    IEnumerator StartScene()
    {
        // 검은 화면에 "10년 후...." 표시
        fadePanel.SetActive(true); // fadePanel 활성화
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 0; // 초기 알파 값 0으로 설정
        fadeImage.color = color;

        yield return StartCoroutine(FadeIn());
        fadeText.text = "10년 후....";
        yield return new WaitForSeconds(2f); // 텍스트 유지 시간
        fadeText.text = "";
        yield return StartCoroutine(FadeOut());

        // 대화 시작
        InitializeDialogues();
        StartDialogue();
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
        isDialogueActive = true; // 대화 활성화 상태 설정
        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if (circle1Sentences.Count == 0 && circle2Sentences.Count == 0)
        {
            StartCoroutine(EndScene());
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

    IEnumerator EndScene()
    {
        dialogueBox.SetActive(false);
        nameBox.SetActive(false);

        yield return StartCoroutine(FadeIn());

        fadeText.text = "THE END";
        fadeText.gameObject.SetActive(true); 
        yield return new WaitForSeconds(3f); 
    }

    IEnumerator FadeIn()
    {
        fadePanel.SetActive(true); 

        float fadeDuration = 1f;
        float elapsedTime = 0f;
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }


    IEnumerator FadeOut()
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadePanel.SetActive(false);
    }
}
