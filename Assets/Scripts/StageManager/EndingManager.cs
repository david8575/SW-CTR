using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float checkInterval = 0.5f;

    private void Start()
    {
        FindObjectOfType<StageT3Manager>().AddBossDeadEvent(GoToEnding);
    }

    private void GoToEnding()
    {
        SceneManager.LoadScene(14);
    }
}
