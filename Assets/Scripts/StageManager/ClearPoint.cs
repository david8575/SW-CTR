using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearPoint : MonoBehaviour
{
    public Timer timer; 
    public GameObject starDisplayUI; 

    private bool isCleared = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCleared)
        {
            isCleared = true; 
            Debug.Log("Stage Cleared");

            GameManager.instance.CollectStar();

            if (timer != null && timer.IsCompletedWithinTime())
            {
                GameManager.instance.CollectStar();
            }

            ShowStarCount();
        }
    }

    private void ShowStarCount()
    {
        if (starDisplayUI != null)
        {
            starDisplayUI.SetActive(true);
            var starText = starDisplayUI.GetComponent<UnityEngine.UI.Text>();
            if (starText != null)
            {
                starText.text = "Stars: " + GameManager.instance.GetStarsCollected();
            }
        }
    }
}
