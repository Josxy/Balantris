using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board mainBoard;

    public Tilemap tilemap { get; private set; }
    public Vector3[] cells { get; private set; }
    public Vector3 position { get; private set; }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector3[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
        
    }

    private void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 tilePosition = cells[i] + position;
            tilemap.SetTile(Vector3Int.RoundToInt(tilePosition), null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = mainBoard.newPiece.cells[i];
        }
    }

    private void Drop()
    {
        Vector3 position = mainBoard.newPiece.position;

        float current = position.y;
        float bottom = -mainBoard.boardBounds.y / 2;

        mainBoard.Clear(mainBoard.newPiece);

        for (float row = current; row >= bottom; row--)
        {
            position.y = row;

            if (mainBoard.isValid(mainBoard.newPiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }
        mainBoard.Set(mainBoard.newPiece);
    }

    private void Set()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 tilePosition = cells[i] + position;
            tilemap.SetTile(Vector3Int.RoundToInt(tilePosition), tile);
        }
    }

}
