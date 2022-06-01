using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject testTile;
    public GameObject testTile2;
    public GameObject[,] tiles;
    GameObject offset;

    static int gridSize = 6;
    static int minChainSize = 3;


    public bool isDragStarted = false;

    //[HideInInspector]
    public List<GameObject> chain;

    public ChainRenderer chainRenderer;
    public GameObject node;


    void Start()
    {
        chain = new List<GameObject>();
        chainRenderer = gameObject.GetComponentInChildren<ChainRenderer>();
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
                GameObject newTile = Instantiate(testTile, new Vector3(offset.transform.position.x + i, offset.transform.position.y + j, offset.transform.position.z), Quaternion.identity, offset.transform);
                newTile.name = "[" + (j + 1).ToString() + "]" + "[" + (i + 1).ToString() + "]";
                newTile.GetComponent<TileBehaviour>().indexY = j;
                newTile.GetComponent<TileBehaviour>().indexX = i;
                tiles[i, j] = newTile;
            }
        }
    }

    public void ChainDone()
    {
        if (chain.Count >= minChainSize)
        {
            foreach (GameObject item in chain)
            {
                for (int i = 0; i < gridSize; i++) //Columns
                {
                    for (int j = 0; j < gridSize; j++) //Rows
                    {
                        if (tiles[i, j] == item)
                        {

                        }
                    }
                }
            }
            GenereteNewTile();

            //Some game logic happens <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        }

        chainRenderer.DestroyNodes();
        chain.Clear(); //Clearing the chain elements
    }

    void GenereteNewTile()
    {
        GameObject[] newTiles = new GameObject[chain.Count];
        float[] numToGenerate = new float[gridSize];
        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                if (chain.Contains(tiles[i, j]))
                {
                    numToGenerate[i]++;
                }
            }
            Debug.Log(numToGenerate[i].ToString());
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < numToGenerate[i]; j++)
            {
                newTiles[i] = Instantiate(testTile2,new Vector3(tiles[i,0].transform.position.x, gridSize + numToGenerate[i]),Quaternion.identity,offset.transform);
            }
        }

        // In borodatiye vremena it was somewhat working 

        //int indexX = chainTile.GetComponent<TileBehaviour>().indexX;
        //int indexY = chainTile.GetComponent<TileBehaviour>().indexY;
        //GameObject newTile = Instantiate(testTile2, new Vector3(chainTile.transform.position.x, offset.transform.position.y + indexY + 1, offset.transform.position.z), Quaternion.identity, offset.transform);
        //newTile.name = tiles[indexX, indexY].name;
        //GameObject.Destroy(tiles[indexX, indexY]);
        //tiles[indexX, indexY] = null;
    }

    void Fall()
    {
        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                if (tiles[i,j] == null)
                {
                    //MoveFirstFromTop(tiles[i, j].GetComponent<TileBehaviour>().indexX, tiles[i, j].GetComponent<TileBehaviour>().indexY);
                }
            }
        }
    }

    void MoveFirstFromTop(int x, int y)
    {
        for (int i = 0; i < y; i++)
        {
            if (tiles[x, i] != null)
            {
                Debug.Log("First: " + tiles[x, i].name);
                //tiles[x, y] = tiles[x, i];
                //tiles[x, y].transform.position = Vector3.Lerp(tiles[x, y].transform.position, tiles[x, i].transform.position, 2f);
                return;
            }
            Debug.Log("There is nothing on top");
        }
    }
}