using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    /*
    public float levelDuration = 10.0f;
    public Text timerText;
    public Text gameText;
    public Text pointText;
    */

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;

    public static bool isGameOver = false;

    public string nextLevel;

    // float countDown;

    int points = 0;

    void Start()
    {
        isGameOver = false;
        /*
        countDown = levelDuration;
        SetTimerText();
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!isGameOver)
        {
            if (countDown > 0)
            {
                countDown -= Time.deltaTime;
            }
            else
            {
                countDown = 0.0f;

                LevelLost();
            }
            SetTimerText();
        }
        */
    }
    /*
    private void SetTimerText()
    {
        timerText.text = countDown.ToString("f2");
    }
    */
    public void LevelLost()
    {
        isGameOver = true;
        // gameText.text = "Game Over!";
        // gameText.gameObject.SetActive(true);

        // Camera.main.GetComponent<AudioSource>().pitch = 1;
        AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);

        Invoke("LoadCurrentLevel", 2);
    }
    public void LevelBeat()
    {
        isGameOver = true;
        // gameText.text = "You Win!";
        // gameText.gameObject.SetActive(true);

        // Camera.main.GetComponent<AudioSource>().pitch = 5;
        AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

        if (!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }

    }

    public void IncrementPoints(int value)
    {
        points += value;
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}