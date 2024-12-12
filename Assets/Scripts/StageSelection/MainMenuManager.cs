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
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
    public void OnResetButton()
    {
        DataManager.Instance.RemoveGameData();
    }
}
