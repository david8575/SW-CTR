using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : StageBase
{
    public DialogueManager dialogueManager;

    protected override void Start()
    {
        FadeManager.Instance.SetFade(true);

        StartCoroutine(StoryCoroutine());
    }

    IEnumerator StoryCoroutine()
    {
        yield return new WaitForSeconds(1f);

        dialogueManager.StartDialouge("test", 0.07f, true);
        yield return new WaitWhile(() => DialogueManager.IsInDialogue);
        yield return new WaitForSeconds(0.5f);

        yield return FadeManager.Instance.FadeOut();

        dialogueManager.StartDialouge("test");
    }
}
