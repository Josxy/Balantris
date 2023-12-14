using UnityEngine.Tilemaps;
using UnityEngine;
using static UnityEngine.UI.Image;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Linq;

public class Board : MonoBehaviour
{
    public Grid grid;
    public TetroData[] pieces;
    public Tile[] allTiles;
    public Tilemap tilemap { get; private set; }
    public Piece newPiece { get; private set; }
    public Camera cam;
    private Vector3 spawnPosition = new Vector3(0,5,0);
    public Vector2 boardBounds = new Vector2(20f, 12f);
    public Vector3 centerOfMass = new Vector3(0f, 0f, 0f);
    public bool gameOver = false;
    public int numberOfTile = 0;
    public int AllnumberOfTiles = 0;
    public int clearedLine = 0;
    public GameObject timeCanvas;
    //Queues for spawning
    public Queue<int> indexHolder = new Queue<int>();
    public Queue<int> alternativeIndexHolder = new Queue<int>();
    private int sequenceSize = 7;
    private int threshHoldQueue = 3;
    //Indexes for hold 
    public int holdedIndex = 10;
    private int currentIndex = 10;
    public bool holded = false;
    
    List<int> list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

    // Converts bounds to Rect
    public Rect Bounds
    {
        get
        {
            Vector2 position = new Vector2(-this.boardBounds.x / 2, -this.boardBounds.y / 2);
            return new Rect(position, this.boardBounds);
        }
    }
    //Createt tetromino data for our board. 
    void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        pieces = new TetroData[]
        {
            new TetroData(Tetrominoes.I, allTiles[0], TetrominoShapes.I, 0,wallKicks.WallKicksI),
            new TetroData(Tetrominoes.O, allTiles[1], TetrominoShapes.O, 0,wallKicks.WallKicksJLOSTZ),
            new TetroData(Tetrominoes.T, allTiles[2], TetrominoShapes.T, 0,wallKicks.WallKicksJLOSTZ),
            new TetroData(Tetrominoes.J, allTiles[3], TetrominoShapes.J, 0,wallKicks.WallKicksJLOSTZ),
            new TetroData(Tetrominoes.L, allTiles[4], TetrominoShapes.L, 0,wallKicks.WallKicksJLOSTZ),
            new TetroData(Tetrominoes.S, allTiles[5], TetrominoShapes.S, 0,wallKicks.WallKicksJLOSTZ),
            new TetroData(Tetrominoes.Z, allTiles[6], TetrominoShapes.Z, 0,wallKicks.WallKicksJLOSTZ)
        };
        QueueEnqueuer();
    }

    void Start()
    {
        Spawner();
    }

    void Update()
    {
        grid.Rotator(this);
        QueueEnqueuer();
    }
    //Enqueues two queues for spawning. first one is main queue and other is the next one. It follows tetris new spawning system
    //It spawnes objects with sequeneces of 7
    private void QueueEnqueuer()
    {
       List<int> temp = new List<int>(list);
        while (indexHolder.Count < sequenceSize)
        {
            for(int i = 0; i < sequenceSize; i++)
            {
                int randomNum = Random.Range(0, temp.Count);
                this.indexHolder.Enqueue(temp[randomNum]);
                temp.RemoveAt(randomNum);
            }
        }
        List<int> temp2 = new List<int>(list);
        while (alternativeIndexHolder.Count < sequenceSize)
        {
            for (int i = 0; i < sequenceSize; i++)
            {
                int randomNum = Random.Range(0, temp2.Count);
                this.alternativeIndexHolder.Enqueue(temp2[randomNum]);
                temp2.RemoveAt(randomNum);
            }
        }
        if(indexHolder.Count < threshHoldQueue)
        {
            while (alternativeIndexHolder.Count > 0)
                indexHolder.Enqueue(alternativeIndexHolder.Dequeue());
        }

    }
    //this function changes pieces and hold the previous piece
   public void Hold()
   {
        if (holdedIndex == 10 && !holded)
        {
            holdedIndex = currentIndex;
            Spawner();
            holded = true;
        }
        else if (holdedIndex != 10 && !holded)
        {
            int tempIndex = holdedIndex;
            holdedIndex = currentIndex;
            currentIndex = tempIndex;
            SpawnWithIndex(currentIndex);
            holded = true;
        }
   }
    //Spawn tetrominoe with spesific index
    public void SpawnWithIndex(int index)
    {
        if(!gameOver)
        {
            TetroData data = this.pieces[index];
            this.newPiece = gameObject.AddComponent<Piece>();
            this.newPiece.Initialize(this, data, this.spawnPosition);
            if (!this.isValid(this.newPiece, Vector3Int.RoundToInt(this.spawnPosition)))
                gameOver = true;
            Set(newPiece);
            this.AllnumberOfTiles += 4;
            this.numberOfTile += 4;
        }
    }
    //Spawn tetrominoes with dequeueing a index from queue.
    public void Spawner()
    {
        if(!gameOver)
        {
            currentIndex = this.indexHolder.Dequeue();
            TetroData data = this.pieces[currentIndex];

            this.newPiece = gameObject.AddComponent<Piece>();
            this.newPiece.Initialize(this, data, this.spawnPosition);
            if(!this.isValid(this.newPiece,Vector3Int.RoundToInt(this.spawnPosition)))
                gameOver = true;
            Set(newPiece);
            this.AllnumberOfTiles += 4;
        }  
    }
    //It set tiles
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3 tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(Vector3Int.RoundToInt(tilePosition), piece.data.tiles);
        }
    }
    //It clear tiles
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3 tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(Vector3Int.RoundToInt(tilePosition), null);
        }
    }
    //checks if the spot is valid or not
    public bool isValid(Piece piece, Vector3 position)
    {
        Rect bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3 tilePosition = piece.cells[i] + position;
            // Check if it is in the bounds of the board.
            if (!bounds.Contains((Vector2)tilePosition))
            {
                return false;
            }
            // Check if it is an empty space or not.
            if (this.tilemap.HasTile(Vector3Int.RoundToInt(tilePosition)))
            {
                return false;
            }
        }

        return true;
    }
    //calculates the center of mass for the rotation
    public void Balance() 
    {
        Rect boundaries = this.Bounds;
        float highestLine = this.highestLine();
        int tileNum = 0;

        Vector3 tilePosition = new Vector3(0, 0, 0);

        for(float row = boundaries.yMin; row <= highestLine; row++)
        {
            for (float col = boundaries.xMin - 1; col < boundaries.xMax; col++)
            { 
                Vector3 position = new Vector3(col, row, 0);
                if (this.tilemap.HasTile(Vector3Int.RoundToInt(position)))
                {
                    tileNum++;
                    if (position.x >= 0)
                        position += new Vector3(1,0,0);
                    if (position.y >= 0)
                        position += new Vector3(0, 1, 0);
                    tilePosition += position;
                }
            }
        }
       
        tilePosition /= tileNum;
        this.numberOfTile = tileNum;
        centerOfMass = tilePosition;
    }
    //clear random line when board in equilibrium
    public void clearLine(float maxRow)
    {
        if(this.newPiece.isDropped)
        {
            Rect newbounds = this.Bounds;

            float row = 0;

            float max = this.highestLine();

            row = Mathf.RoundToInt(Random.Range(-8, max));

            for (float column = newbounds.xMin; column < newbounds.xMax; column++)
            {
                Vector3 position = new Vector3(column, row, 0);
                this.tilemap.SetTile(Vector3Int.RoundToInt(position), null);
            }

            while (row < newbounds.yMax)
            {
                for (float column = newbounds.xMin; column < newbounds.xMax; column++)
                {
                    Vector3 position = new Vector3(column, row + 1, 0);
                    TileBase above = this.tilemap.GetTile(Vector3Int.RoundToInt(position));

                    position = new Vector3(column, row, 0);
                    this.tilemap.SetTile(Vector3Int.RoundToInt(position), above);
                }
                row++;
            }
            this.clearedLine++;
            this.Balance();
            this.Spawner();
            newPiece.isDropped = false;
        }
    }
    //clear row when a row is full
    public void clearRow()
    {
        Rect boundaries = this.Bounds;
        float row = boundaries.yMin;

        while(row < boundaries.yMax) 
        {
            if (this.isRowFull(row))
                clearHorizontal(row);
            else
                row++;
        }
        Balance();
    }
    //checks if the row is full or not
    private bool isRowFull(float row)
    {
        Rect boundaries = this.Bounds;

        for(float column = boundaries.xMin; column < boundaries.xMax;column++)
        {
            Vector3 position = new Vector3(column, row, 0);
            if(!this.tilemap.HasTile(Vector3Int.RoundToInt(position)))
                return false;
        }
        return true;
    }
    //clear the fulled row and shift it the bottom
    public void clearHorizontal(float row)
    {
        Rect boundaries = this.Bounds;

        for (float column = boundaries.xMin; column < boundaries.xMax; column++)
        {
            Vector3 position = new Vector3(column, row, 0);
            numberOfTile--;
            this.tilemap.SetTile(Vector3Int.RoundToInt(position), null);
        }
        while(row < boundaries.yMax)
        {
            for (float column = boundaries.xMin; column < boundaries.xMax; column++)
            {
                Vector3 position = new Vector3(column, row + 1, 0);
                TileBase above = this.tilemap.GetTile(Vector3Int.RoundToInt(position));

                position = new Vector3(column, row, 0);
                this.tilemap.SetTile(Vector3Int.RoundToInt(position), above);
            }
            row++;
        }
    }
    //it calculates the highest line for some functions
    public float highestLine()
    {
        Rect boundaries = this.Bounds;
        
        float highestrow = boundaries.yMin;

        for (float row = boundaries.yMin; row < boundaries.yMax; row++)
        {
            bool isLineEmpty = true;

            for (float column = boundaries.xMin; column < boundaries.xMax; column++)
            {
                Vector3 position = new Vector3(column, row, 0);
                if (tilemap.HasTile(Vector3Int.RoundToInt(position)))
                {
                    isLineEmpty = false;
                    break; // No need to check other tiles in the row if we find a non-empty one.
                }
            }

            if (!isLineEmpty)
                highestrow = row;
            else
                break;
        }
        return highestrow;
    }
}