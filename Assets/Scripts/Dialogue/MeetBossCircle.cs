using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 추가

public class MeetBossCircle : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string characterName;
        [TextArea] public string[] sentences;
        public Sprite characterSprite;
    }

    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject dialoguePanel;
    public GameObject namePanel;
    public GameObject fadePanel;
    public Image characterImage;
    public Dialogue bigCircleDialogue;
    public Dialogue smallCircleDialogue;
    public GameObject bigCircle;
    public GameObject smallCircle;
    public GameObject square;
    public GameObject triangle;
    public int targetBuildIndex = 1; 

    private Queue<string> bigCircleSentences;
    private Queue<string> smallCircleSentences;
    private bool isBigCircleTurn = true;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    void Start()
    {
        bigCircleSentences = new Queue<string>();
        smallCircleSentences = new Queue<string>();
        dialoguePanel.SetActive(false);
        fadePanel.SetActive(false);
        square.SetActive(false);
        triangle.SetActive(false);
        FadeManager.Instance.FadeOut();

        StartCoroutine(StartDialogueOnce());
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextSentence();
        }
    }

    IEnumerator StartDialogueOnce()
    {
        dialoguePanel.SetActive(true);
        InitializeDialogueQueues();
        DisplayNextSentence();
        yield return StartCoroutine(DisplayDialogue());

        yield return StartCoroutine(EndDialogue());
    }

    IEnumerator DisplayDialogue()
    {
        while (isDialogueActive)
        {
            yield return null;
        }
    }

    void InitializeDialogueQueues()
    {
        bigCircleSentences.Clear();
        foreach (string sentence in bigCircleDialogue.sentences)
        {
            bigCircleSentences.Enqueue(sentence);
        }

        smallCircleSentences.Clear();
        foreach (string sentence in smallCircleDialogue.sentences)
        {
            smallCircleSentences.Enqueue(sentence);
        }

        isDialogueActive = true;
    }

    void DisplayNextSentence()
    {
        if (bigCircleSentences.Count > 0 || smallCircleSentences.Count > 0)
        {
            if (isBigCircleTurn)
            {
                if (bigCircleSentences.Count > 0)
                {
                    DisplaySentence(bigCircleDialogue.characterName, bigCircleDialogue.characterSprite, bigCircleSentences.Dequeue());
                    isBigCircleTurn = false;
                }
                else if (smallCircleSentences.Count > 0)
                {
                    isBigCircleTurn = false;
                    DisplayNextSentence();
                }
            }
            else
            {
                if (smallCircleSentences.Count > 0)
                {
                    DisplaySentence(smallCircleDialogue.characterName, smallCircleDialogue.characterSprite, smallCircleSentences.Dequeue());
                    isBigCircleTurn = true;
                }
                else if (bigCircleSentences.Count > 0)
                {
                    isBigCircleTurn = true;
                    DisplayNextSentence();
                }
            }
        }

        if (bigCircleSentences.Count == 0 && smallCircleSentences.Count == 0)
        {
            isDialogueActive = false;
            StartCoroutine(EndDialogue());
        }
    }

    void DisplaySentence(string characterName, Sprite characterSprite, string sentence)
    {
        nameText.text = characterName;
        characterImage.sprite = characterSprite;
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

    IEnumerator EndDialogue()
    {
        dialoguePanel.SetActive(false);
        namePanel.SetActive(false);

        yield return StartCoroutine(ShrinkAndDisappear(bigCircle));

        yield return StartCoroutine(EnlargeAndShow(square));
        yield return StartCoroutine(EnlargeAndShow(triangle));

        yield return StartCoroutine(FadeIn());

        SceneManager.LoadScene(targetBuildIndex); // 빌드 인덱스로 이동
    }

    IEnumerator ShrinkAndDisappear(GameObject obj)
    {
        float shrinkDuration = 1f; 
        float elapsedTime = 0f;
        Vector3 originalScale = obj.transform.localScale;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration);
            obj.transform.localScale = originalScale * scale;
            yield return null;
        }

        obj.SetActive(false);
    }

    IEnumerator EnlargeAndShow(GameObject obj)
    {
        obj.SetActive(true); 
        float enlargeDuration = 1f; 
        float elapsedTime = 0f;
        Vector3 targetScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero; 

        while (elapsedTime < enlargeDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(0f, 1f, elapsedTime / enlargeDuration);
            obj.transform.localScale = targetScale * scale;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        fadePanel.SetActive(true);
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadePanel.SetActive(false);
    }
}
