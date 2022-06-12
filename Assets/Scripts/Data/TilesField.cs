using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesField : MonoBehaviour
{
    public static int gridSize = 6; //Size of both dimentions

    public GameObject[,] tiles = new GameObject[gridSize, gridSize]; //Tiles matrix
    public GameObject[,] extendedTiles = new GameObject[gridSize, gridSize]; //Extended tiles matrix

    void Start()
    {
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
