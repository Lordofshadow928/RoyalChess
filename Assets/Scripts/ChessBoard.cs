using Unity.VisualScripting;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    //Logic for generating a chessboard grid of tiles
    private const int TileCountX = 8;
    private const int TileCountY = 8;
    private GameObject[,] tiles;

    private void Awake()
    {
        GeneratedAllTiles(1, TileCountX, TileCountY);
    }

    private void GeneratedAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                tiles[x, y] = GeneratedSingleTiles(tileSize, x, y);
    }
    private GameObject GeneratedSingleTiles(float tileSize, int x, int y)
    {
        GameObject tile = new GameObject($"Tile_{x}_{y}");
        tile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);
        tile.transform.localScale = new Vector3(tileSize, 1, tileSize);
        return tile;
    }
}
