using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Screen.fullScreen = PlayerPrefs.GetInt("FullScreen", 0) == 1 ? true : false;
    }

    public void OnStartButton()
    {
        if (DataManager.Instance.SaveData.IsTutorialClear == false)
        {
            StartCoroutine(LoadScene(2));
        }
        else
        {
            StartCoroutine(LoadScene(1));
        }
    }

    IEnumerator LoadScene(int idx)
    {
        var load = SceneManager.LoadSceneAsync(idx);
        load.allowSceneActivation = false;

        yield return FadeManager.Instance.FadeIn();

        load.allowSceneActivation = true;
    }

    public void OnResetButton()
    {
        DataManager.Instance.RemoveGameData();
    }
}
