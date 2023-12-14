using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameScore : MonoBehaviour
{
    public TextMeshProUGUI scoreboard;
    float score;
    public Canvas canvas;

    private void Update()
    {
        score = canvas.GetComponent<ScoreBoard>().score;
        scoreboard.text = Mathf.RoundToInt(score).ToString();
    }
}
