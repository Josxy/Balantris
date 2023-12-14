using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreboard;
    float score;
    public Canvas canvas;
    public TextMeshProUGUI highscore;

    private void Start()
    {
        score = canvas.GetComponent<ScoreBoard>().score;
        scoreboard.text = Mathf.RoundToInt(score).ToString();
        highscore.text = Mathf.RoundToInt(PlayerPrefs.GetFloat("HighScore", 0)).ToString();
    }

    private void Update()
    {
        if(score > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", Mathf.RoundToInt(score));
            highscore.text = Mathf.RoundToInt(score).ToString();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void ExitGame()
    {
        SceneManager.LoadScene("NewMenu");
    }
}
