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
                newTile.GetComponent<SpriteRenderer>().transform.Rotate(0, 0, Random.Range(-10f, 10f), Space.Self);
                tiles[i, j] = newTile;
            }
        }
    }

    public void ChainDone()
    {
        if (chain.Count > minChainSize)
        {
            foreach (GameObject item in chain)
            {
                for (int i = 0; i < gridSize; i++) //Columns
                {
                    for (int j = 0; j < gridSize; j++) //Rows
                    {
                        if (tiles[i, j] == item)
                        {
                            tiles[i, j].GetComponent<SpriteRenderer>().material.color = tiles[i, j].GetComponent<TileBehaviour>().defaultColor;
                        }
                    }
                }
                GenereteNewTile(item);
                Fall(item);
            }
            //Some game logic happens
        }
        else
        {
            for (int i = 0; i < gridSize; i++) //Columns
            {
                for (int j = 0; j < gridSize; j++) //Rows
                {
                    tiles[i, j].GetComponent<SpriteRenderer>().material.color = tiles[i, j].GetComponent<TileBehaviour>().defaultColor;
                }
            }
        }

        chainRenderer.DestroyNodes();
        chainRenderer.lr.positionCount = 0;
        chain.Clear(); //Clearing the chain elements
    }

    void GenereteNewTile(GameObject chainTile)
    {
        int indexX = chainTile.GetComponent<TileBehaviour>().indexX;
        int indexY = chainTile.GetComponent<TileBehaviour>().indexY;

        GameObject newTile = Instantiate(testTile2, new Vector3(chainTile.transform.position.x, offset.transform.position.y + indexY + 1, offset.transform.position.z), Quaternion.identity, offset.transform);
        newTile.name = tiles[indexX, indexY].name;


        GameObject.Destroy(tiles[indexX, indexY]);
        tiles[indexX, indexY] = null;
    }

    void Fall(GameObject chainTile)
    {
        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                if (tiles[i, j] == null)
                {
                    //MoveFirstFromTop(tiles[i + 1, j]);
                    //StartCoroutine(FallAnimation());
                }
            }
        }
    }

    IEnumerator FallAnimation(GameObject fallingObject, Vector3 endPosition)
    {
        float duration = 1f;

        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            fallingObject.transform.Translate((endPosition + fallingObject.transform.position) * Time.deltaTime, Space.World);
        }
        yield return null;
    }

    int MoveFirstFromTop(GameObject position)
    {
        for (int i = position.GetComponent<TileBehaviour>().indexY; i < gridSize; i++)
        {
            if (tiles[i, position.GetComponent<TileBehaviour>().indexX] != null)
            {
                //Debug.Log("First on top: " + i.ToString());
                return i;
            }
        }
        return position.GetComponent<TileBehaviour>().indexY;
    }
}