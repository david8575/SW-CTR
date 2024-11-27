using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public StageTimer stageTimer;
    public TextMeshProUGUI starText;

    public GameObject[] clearObjects;
    public GameObject[] stars;
    public TextMeshProUGUI[] timeTexts;
    public TextMeshProUGUI puzzleText;

    public Button finishButton;

    private void Start()
    {
        for (int i = 0; i < clearObjects.Length; i++)
        {
            if (i < stars.Length)
                stars[i].SetActive(false);
            clearObjects[i].SetActive(false);
        }
        
    }

    public IEnumerator StageCorutine()
    {
        var wait = new WaitForSeconds(1f);
        int i;

        for (i = 0; i < 4; i++)
        {
            clearObjects[i].SetActive(true);
            yield return wait;
        }

        for (int j = 0; j < GameManager.instance.starsCollected; j++)
        {
            stars[j].SetActive(true);
            yield return wait;
        }

        clearObjects[i].SetActive(true);
    }
}
