using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : StageBase
{
    public DialogueManager dialogueManager;

    public GameObject[] cameraArray;

    protected override void Awake()
    {
        FadeManager.Instance.SetFade(true);

        StartCoroutine(StoryCoroutine());
    }

    IEnumerator StoryCoroutine()
    {
        yield return new WaitForSeconds(1f);

        dialogueManager.StartDialouge("intro", 0.07f, true);
        yield return new WaitWhile(() => DialogueManager.IsInDialogue);
        PlayerController.Instance.SetInputSystem(false);
        yield return new WaitForSeconds(0.5f);

        cameraArray[0].SetActive(false);
        cameraArray[1].SetActive(true);

        yield return FadeManager.Instance.FadeOut();

        yield return new WaitForSeconds(0.5f);

        dialogueManager.StartDialouge("first_chat");

        yield return new WaitWhile(() => DialogueManager.IsInDialogue);
        
        yield return new WaitForSeconds(1f);




    }
}
