using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject testTile;
    public GameObject testTile2;
    public GameObject[,] tiles;
    GameObject offset;

    static int gridSize = 8;
    static int minChainSize = 3;
    static int maxChainSize = 54;


    public bool isDragStarted = false;

    //[HideInInspector]
    public List<GameObject> chain;


    void Start()
    {
        chain = new List<GameObject>();
        FirstGenerate();
    }

    void FirstGenerate()
    {
        offset = GameObject.Find("TileOffset");
        tiles = new GameObject[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                GameObject newTile = Instantiate(testTile, new Vector3(offset.transform.position.x + i, offset.transform.position.y - j, offset.transform.position.z), Quaternion.identity, offset.transform);
                newTile.name = "[" + (j + 1).ToString() + "]" + "[" + (i + 1).ToString() + "]";
                newTile.GetComponent<TileBehaviour>().indexY = j;
                newTile.GetComponent<TileBehaviour>().indexX = i;
                newTile.GetComponent<SpriteRenderer>().transform.Rotate(0, 0, Random.Range(-10f, 10f), Space.Self);
                tiles[i, j] = newTile;
            }
        }
    }

    public void ChainDone()
    {
        foreach (GameObject item in chain)
        {
            for (int i = 0; i < gridSize; i++) //Columns
            {
                for (int j = 0; j < gridSize; j++) //Rows
                {
                    if (tiles[i, j] == item) tiles[i, j].GetComponent<SpriteRenderer>().material.color = tiles[i, j].GetComponent<TileBehaviour>().defaultColor;
                }
            }
            GenereteNewTile(item);
            Fall(item);
        }


        //Some game logic happens


        chain.Clear(); //Clearing the chain elements
    }

    void GenereteNewTile(GameObject chainTile)
    {
        int indexX = chainTile.GetComponent<TileBehaviour>().indexX;
        int indexY = chainTile.GetComponent<TileBehaviour>().indexY;

        GameObject newTile = Instantiate(testTile2, new Vector3(chainTile.transform.position.x, offset.transform.position.y + indexY + 1, offset.transform.position.z), Quaternion.identity, offset.transform);
        newTile.name = tiles[indexX, indexY].name;


        //GameObject.Destroy(tiles[indexX, indexY]);
        //tiles[indexX, indexY] = null;
    }

    void Fall(GameObject chainTile)
    {
        
    }

    int FindFirstOnTop(GameObject startObject)
    {
        return 0;
    }
}