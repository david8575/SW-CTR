using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectTutorial : MonoBehaviour
{

    public Transform[] chats;
    PlayerInputActions inputActions;
    bool mouseClick = false;
    WaitWhile waitMouse;

    private void Start()
    {
        if (DataManager.Instance.SaveData.IsTutorialClear == true)
        {
            gameObject.SetActive(false);
            return;
        }
        Debug.Log("Tutorial Start");

        inputActions = new PlayerInputActions();
        inputActions.DialougeActions.NextDialouge.performed += _ => mouseClick = true;
        waitMouse = new WaitWhile(() => !mouseClick);

        StartCoroutine(TutorialCoroution());
    }

    IEnumerator TutorialCoroution()
    {
        yield return StartCoroutine(TutoMsg(chats[0]));

        inputActions.Dispose();
        DataManager.Instance.SaveData.IsTutorialClear = true;
        DataManager.Instance.SaveGameData();
    }

    IEnumerator TutoMsg(Transform tuto)
    {
        int cnt = tuto.childCount;
        GameObject[] objects = new GameObject[cnt];
        for (int i = 0; i < cnt; i++)
        {
            objects[i] = tuto.GetChild(i).gameObject;
        }

        inputActions.Enable();
        objects[0].SetActive(true);
        for (int i = 1; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
            mouseClick = false;
            yield return waitMouse;
            objects[i].SetActive(false);
        }
        objects[0].SetActive(false);
        inputActions.Disable();
    }
}
