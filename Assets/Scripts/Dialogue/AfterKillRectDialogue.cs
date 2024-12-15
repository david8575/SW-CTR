using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterKillRectDialogue : MonoBehaviour
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

    public Dialogue dialogue;
    public Transform circle;
    public Transform square;
    public Transform newSquarePrefab;
    public float moveSpeed = 5f;
    public float scaleSpeed = 2f;
    private Queue<string> sentences;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool isAbsorptionTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);

        FadeManager.Instance.FadeOut();

        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextSentence();
            Debug.Log("스페이스바 입력 감지");
        }
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        nameText.text = "원";
        sentences.Clear();

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

        if (!isAbsorptionTriggered && sentence.Contains("[도형흡수]"))
        {
            StartCoroutine(AbsorbEffect());
            isAbsorptionTriggered = true; 
            return;
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; 
        dialogueText.text = "";
        
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false; 
    }

    IEnumerator AbsorbEffect()
    {
        // 원이 사각형으로 이동
        while (Vector3.Distance(circle.position, square.position) > 0.1f)
        {
            circle.position = Vector3.MoveTowards(circle.position, square.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 흡수 효과: 원 축소, 사각형 확대
        Vector3 originalCircleScale = circle.localScale;
        Vector3 originalSquareScale = square.localScale;
        float elapsed = 0f;
        float duration = 1f; // 흡수 효과 지속 시간

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 사각형 크기 축소
            square.localScale = Vector3.Lerp(originalCircleScale, Vector3.zero, elapsed / duration);

            yield return null;
        }

        square.gameObject.SetActive(false); // 기존 사각형 비활성화

        // 새로운 사각형 생성
        Instantiate(newSquarePrefab, circle.position, Quaternion.identity);

        Debug.Log("흡수 효과 완료");
    }

    void EndDialogue()
    {
        dialogueText.text = ""; // 대화 텍스트 초기화
        nameText.text = ""; // 이름 텍스트 초기화
        dialoguePanel.SetActive(false); // 대화창 비활성화
        nameBox.SetActive(false);

        SceneManager.LoadScene("Story2");
    }
}
