using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour
{
    public Board board;
    public TetroData data;
    public Vector3[] cells { get; private set; }
    public Vector3 position { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    public bool isDropped = false;

    private float timer = 0f;
    private float stepTime;
    private float moveTime;
    private float lockTime;
    
    
    //Initialize the piece
    public void Initialize(Board board, TetroData data, Vector3 position)
    {
        this.board = board;
        this.data = data;
        this.position = position;
        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;

        if (this.cells == null || this.cells.Length != data.pieces[data.currentShapeIndex].Length)
        {
            this.cells = new Vector3[data.pieces[data.currentShapeIndex].Length];
        }

        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = new Vector3(data.pieces[data.currentShapeIndex][i].x, data.pieces[data.currentShapeIndex][i].y, 0);
        }
    }

    void Update()
    {
        if(!this.board.gameOver)
        {

            this.board.Clear(this);

            lockTime += Time.deltaTime;

            float scroller = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scroller > 0 || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                this.Rotation(1);
            }
            else if (scroller < 0 || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                this.Rotation(-1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.hardDrop();
                board.highestLine();
            }

            if (Time.time > moveTime)
            {
                Mover();
            }

            if (Time.time > stepTime)
            {
                Step();
            }

            if(board.centerOfMass.x == 0)
            {
                timer += Time.deltaTime;
                if(timer > 4.9f && board.grid.isStarted)
                {
                    spesificHardDrop();
                    isDropped = true;
                }
                    
            }
                
            this.board.Set(this);
        }   
    }
    //Make piece goes downwards automatically
    private void Step()
    {
        stepTime = Time.time + stepDelay;

        Movement(Vector2Int.down);
        // Once the piece has been inactive for too long it becomes locked
        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }
    //Handles the movement
    void Mover()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            this.Movement(Vector2.left);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            this.Movement(Vector2.right);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            this.Movement(Vector2.down);
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(!this.board.holded)
            {
                this.position = new Vector3(50, 50, 0);
                Destroy(this);
                this.board.Hold();
            }
        }
    }
    //make the movement
    bool Movement(Vector2 movement)
    {
        Vector3 newPosition = this.position;
        newPosition.x += movement.x;
        newPosition.y += movement.y;

        bool valid = this.board.isValid(this, Vector3Int.RoundToInt(newPosition));
        if (valid)
        {
            this.position = newPosition;
            moveTime = Time.time + moveDelay;
            lockTime = 0f; // reset
        }

        return valid;
    }
    //drops piece to bottom
    public void hardDrop()
    {
        while (Movement(Vector2.down))
            continue;
        Lock();
    }

    //hardDrop for spesific case that board in equiblirium
    public void spesificHardDrop()
    {
        while (Movement(Vector2.down))
            continue;
        spesificLock();
    }
    //locks the piece
    private void Lock()
    {
        this.board.Set(this);
        this.board.clearRow();
        this.board.Balance();
        this.board.Spawner();
        Destroy(this);
        this.board.holded = false;
    }
    //lock for spesific hardDrop
    private void spesificLock()
    {
        this.board.Set(this);
        Destroy(this);
    }
    //make the rotations of tetrominoes
    void Rotation(int rotation)
    {
        int originalIndex = this.data.currentShapeIndex;
        int rotationIndex = Wrap(this.data.currentShapeIndex + rotation, 0, 4);

        this.data.currentShapeIndex = rotationIndex;

        RotationMatrix(rotation);
        if(!this.testWallKicks(this.data.currentShapeIndex, rotation))
        {
            this.data.currentShapeIndex = originalIndex;
            this.RotationMatrix(-rotation);
        }

    }

   void RotationMatrix(int rotation)
    { 
        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = new Vector3(data.pieces[this.data.currentShapeIndex][i].x, data.pieces[this.data.currentShapeIndex][i].y, 0);
        }
    }
    //tests the wall kicks
    bool testWallKicks(int rotationIndex,int rotationDirection)
    {
        int wallKickIndex = getWallKickIndex(rotationIndex,rotationDirection);

        for(int i = 0; i < this.data.wallkicks.GetLength(1);i++)
        {
            Vector2 translation = this.data.wallkicks[wallKickIndex, i];

            if(this.Movement(translation))
            {
                return true;
            }
        }
        return false;
    }
    //gets the needed wall kick
    int getWallKickIndex(int rotationIndex,int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallkicks.GetLength(0));
    }
    //wraps the index
    int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
