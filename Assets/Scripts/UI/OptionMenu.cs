using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{

    public static bool CanPause { get; set; } = true;
    public static bool IsPaused { get; private set; } = false;

    GameObject optionPanel;

    public Toggle fullScreenToggle;
    public Slider soundSlider;
    public Slider musicSlider;

    public AudioManager audioManager;

    private void Start()
    {
        optionPanel = transform.GetChild(0).gameObject;
        optionPanel.gameObject.SetActive(false);

        audioManager = GameManager.Instance.audioManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.activeSelf)
            {
                OnContinueButton();
            }
            else
            {
                Paused();
            }
        }
    }

    void Paused()
    {
        if (!CanPause) return;

        optionPanel.SetActive(true);
        Time.timeScale = 0;
        IsPaused = false;

        fullScreenToggle.isOn = Screen.fullScreen;
        
    }

    public void OnContinueButton()
    {
        optionPanel.SetActive(false);
        Time.timeScale = 1;
        IsPaused = true;
    }

    public void OnQuitButton()
    {
        // ¾À ÀÎµ¦½º °¡Á®¿À±â
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index < 2)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(1);
            OnContinueButton();
        }
    }

    public void OnToggleFullScreen()
    {
        Screen.fullScreen = fullScreenToggle.isOn;
        PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnSoundChange()
    {
        audioManager.SetSoundVolume(soundSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        PlayerPrefs.Save();
    }

    public void OnMusicChange()
    {
        audioManager.musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
    }
}
