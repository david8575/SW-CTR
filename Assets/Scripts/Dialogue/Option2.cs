using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Option2 : MonoBehaviour
{
    public GameObject fadePanel; 
    public TMP_Text fadeText; 
    public List<string> messages; 
    public float textDisplayDuration = 2f; 

    private int currentMessageIndex = 0; 

    void Start()
    {
        fadePanel.SetActive(true); 
        fadeText.text = ""; 
        StartCoroutine(ShowMessages());
    }

    IEnumerator ShowMessages()
    {
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 1; 
        fadeImage.color = color;

        for (currentMessageIndex = 0; currentMessageIndex < messages.Count; currentMessageIndex++)
        {
            fadeText.text = messages[currentMessageIndex]; 
            yield return new WaitForSeconds(textDisplayDuration); 
        }

        fadeText.text = "THE END";
        yield return new WaitForSeconds(3f);
    }
}
