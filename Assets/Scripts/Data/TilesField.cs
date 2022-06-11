using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesField : MonoBehaviour
{
    public GameObject[,] tiles; //Tiles matrix
    public GameObject[,] extendedTiles; //Extended tiles matrix

    public static int gridSize = 6; //Size of both dimentions
    void Start()
    {
        tiles = new GameObject[gridSize, gridSize];
        extendedTiles = new GameObject[gridSize, gridSize];
    }

    public void Clear()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Destroy(tiles[i, j]);
            }
        }
    }
}
