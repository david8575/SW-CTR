using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class StageT3Manager : StageBase
{
    public CinemachineVirtualCamera bossCam;
    public GameObject bossTriangle;

    protected override void Start()
    {
        base.Start();

        isBoss = true;

        bossTriangle.transform.GetChild(0).GetComponent<BossTriangle>().DeadEvent += GameManager.instance.StageClear;

        StartCoroutine(StartCutscene());
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
}
