using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI board;
    public Board gameBoard;
    public Grid grid;
    public float score;
    float weight = 0f;
    float currentTime = 0f;
    //calculates the score
    private void Update()
    {
        currentTime += Time.deltaTime;
        weight = gameBoard.AllnumberOfTiles;
        score = weight + (4 * currentTime) + (20 * gameBoard.clearedLine);
        board.text = "Score: " + Mathf.RoundToInt(score).ToString();
        if(score > 4000)
        {
            grid.dividerConstant = 0.5f;
        }
        else if(score > 2000)
        {
            grid.dividerConstant = 1f;
        }
        else if (score > 1000)
        {
            grid.dividerConstant = 2f;
        }
    }
}
