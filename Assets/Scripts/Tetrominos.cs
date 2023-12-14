using UnityEngine;
using UnityEngine.Tilemaps;
//Enum for tetrominoes
public enum Tetrominoes{I,O,T,J,L,S,Z}
//Holds tetrominoes as a structure
[System.Serializable]
public struct TetroData
{
    public Tetrominoes tetros;
    public Tile tiles;
    public Vector2Int[][] pieces;
    public Vector2Int[,] wallkicks;
    public int currentShapeIndex;
    public TetroData(Tetrominoes tetro, Tile tile, Vector2Int[][] rotations,int newShape, Vector2Int[,] wallkicks)
    {
        this.tetros = tetro;
        this.tiles = tile; 
        this.pieces = rotations;
        this.currentShapeIndex = newShape;
        this.wallkicks = wallkicks;
    }
}
//Holds the data for each tetromino with their rotations
public class TetrominoShapes
{
    public static Vector2Int[][] I = new Vector2Int[][]
    {
        new Vector2Int[]{ new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(0,-2) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(-1,0), new Vector2Int(-2,0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(0,2) }
    };
    public static Vector2Int[][] O = new Vector2Int[][]
    {
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,1) },
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,1) },
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,1) },
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,1) }
    };
    public static Vector2Int[][] T = new Vector2Int[][]
    {
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(-1,0), new Vector2Int(1,0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(1,0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(-1,0), new Vector2Int(1,0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(-1,0) },
    };
    public static Vector2Int[][] J = new Vector2Int[][]
    {
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,1), new Vector2Int(1,0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(1,1) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(-1,0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,-1 ), new Vector2Int(-1,-1) },
    };
    public static Vector2Int[][] L = new Vector2Int[][]
    {
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(-1,0) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(1,-1) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(1,0), new Vector2Int(-1,-1) },
        new Vector2Int[]{ new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(-1,1), new Vector2Int(0,-1) },

    };
    public static Vector2Int[][] S = new Vector2Int[][]
    {
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(-1,0) },
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,-1) },
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,-1), new Vector2Int(-1,-1) },
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(0,-1), new Vector2Int(-1,1) },
    };
    public static Vector2Int[][] Z = new Vector2Int[][]
    {
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(-1,1), new Vector2Int(1,0), new Vector2Int(0,1) },
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,-1) },
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(0,-1), new Vector2Int(1,-1) },
        new Vector2Int[] {new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(-1,0), new Vector2Int(-1,-1) },
    };
};
//Holds the wall kicks data for tetrominos. This datas are coming from SRS(super rotation system)
public class wallKicks
{
    public static Vector2Int[,] WallKicksI = new Vector2Int[,]
    {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
    };

    public static Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,]
    {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
    };
}