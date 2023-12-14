using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour
{
    Vector3 origin = new Vector3(0, 0, 0);
    public Ghost ghost;
    public GameObject canvas;
    public GameObject ScoreBoard;
    public GameObject gameOverCanvas;
    public bool isStarted = false;
    public float rotationAngle = 0f;
    public float dividerConstant = 4f;
    
    private void Start()
    {
        canvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }
    //rotates the grid
    public void Rotator(Board board)
    {
        //rotation speed
        this.rotationAngle = -1f * board.AllnumberOfTiles * board.centerOfMass.x / dividerConstant;
        //if it is in the equiblruim it clears a random line to broke the equiblruim
        if (board.centerOfMass.x == 0 && isStarted)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 0.05f);
            board.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 0.05f);
            ghost.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 0.05f);
            
            float maxRow = board.highestLine();

            canvas.SetActive(true);
            canvas.GetComponent<Timer>().startCount();
            if (canvas.GetComponent<Timer>().currentTime <= 0f)
            {
                board.clearLine(maxRow);
                canvas.GetComponent<Timer>().resetCount();
            }
        }
        else
        {
            canvas.SetActive(false);
            transform.RotateAround(this.origin, Vector3.forward, this.rotationAngle * Time.deltaTime);
            board.transform.RotateAround(this.origin, Vector3.forward, this.rotationAngle * Time.deltaTime);
            ghost.transform.RotateAround(this.origin, Vector3.forward, this.rotationAngle * Time.deltaTime);
            canvas.GetComponent<Timer>().resetCount();
        }

        if (!isStarted && transform.rotation.z != 0)
            isStarted = true;
        //game over conditions
        if ((transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270) || board.gameOver)
        {
            gameOverCanvas.SetActive(true);
            ScoreBoard.SetActive(false);
            board.gameOver = true;
        }
    }
}
