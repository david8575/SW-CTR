using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class StageTBManager : MonoBehaviour
{
    public CinemachineVirtualCamera bossCam;
    public GameObject bossTriangle;

    private void Start()
    {
        StartCoroutine(StartCutscene());

        bossTriangle.transform.GetChild(0).GetComponent<BossTriangle>().DeadEvent += () => StartCoroutine(StageClear());
    }

    IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(0.5f);

        PlayerController.Instance.SetInputSystem(false);

        PlayerController.Instance.vcam.gameObject.SetActive(false);

        bossCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        bossTriangle.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        bossCam.gameObject.SetActive(false);
        PlayerController.Instance.vcam.gameObject.SetActive(true);

        PlayerController.Instance.SetInputSystem(true);

    }

    IEnumerator StageClear()
    {
        PlayerController.Instance.SetInputSystem(false);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("StageSelect");
    }
}
