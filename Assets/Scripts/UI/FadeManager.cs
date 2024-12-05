using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Image fade;
    float fadeSpeed = 0.01f;

    public bool IsFade
    {
        get { return fade.gameObject.activeSelf; }
    }

        WaitForSeconds wait = new WaitForSeconds(0.01f);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetFade(bool isOn)
    {
        fade.gameObject.SetActive(isOn);
        float a = isOn ? 1 : 0;
        fade.color = new Color(0, 0, 0, a);
    }

    public Coroutine FadeIn(float speed = 0.01f)
    {
        fadeSpeed = speed;
        return StartCoroutine(FadeInCorutine());
    }

    IEnumerator FadeInCorutine()
    {
        float a = 0;
        fade.gameObject.SetActive(true);
        while (a < 1)
        {
            a += fadeSpeed;
            fade.color = new Color(0, 0, 0, a);
            yield return wait;
        }
    }

    public Coroutine FadeOut(float speed = 0.01f)
    {
        fadeSpeed = speed;
        return StartCoroutine(FadeOutCorutine());
    }

    IEnumerator FadeOutCorutine()
    {
        float a = 1;
        fade.gameObject.SetActive(true);
        while (a > 0)
        {
            a -= fadeSpeed;
            fade.color = new Color(0, 0, 0, a);
            yield return wait;
        }
        fade.gameObject.SetActive(false);
    }

}
