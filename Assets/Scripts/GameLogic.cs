using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject testTile;  //Test tile prefab for first board filling
    public GameObject testTile2; //Test tile prefab for generation
    public GameObject tempTile;  //NULL tile prefab

    public GameObject[,] tiles; //Tiles matrix
    public GameObject[,] extendedTiles; //Extended tiles matrix
    GameObject offset; //Tiles matrix offset

    static int gridSize = 6; //Size of both dimentions
    static int minChainSize = 3; // Minimal chain size

    public bool isDragStarted = false; //Boolean for detection of draging the chain
    public bool IsShifting = false; //Boolean for detection of starting shift down (unuseble)
    [SerializeField] float duration = 1f; //Duration of shifting

    public List<GameObject> chain; //List for chain elements

    //Chain visuals
    public ChainRenderer chainRenderer; //ChainRenderer script
    public GameObject node; //Prefab for head and tail of chain

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
        extendedTiles = new GameObject[gridSize, gridSize * 2];

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
            GenereteNewTiles();

            //Some game logic happens <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        }

        chainRenderer.DestroyNodes();
        chain.Clear(); //Clearing the chain elements
    }

    void GenereteNewTiles()
    {
        int[] numToGen = new int[gridSize];

        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                if (chain.Contains(tiles[i, j]))
                {
                    numToGen[i]++;
                    GameObject.Destroy(tiles[i, j]);
                    tiles[i, j] = null;
                }
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < numToGen[i]; j++)
            {
                extendedTiles[i, j + gridSize] = Instantiate(testTile2, new Vector3(offset.transform.position.x + i, offset.transform.position.y + gridSize + j, offset.transform.position.z), Quaternion.identity, offset.transform);
                extendedTiles[i, j + gridSize].name = "[" + (j + 1).ToString() + "]" + "[" + (i + 1).ToString() + "] TMP";
            }
        }

        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                if (tiles[i, j] == null)
                {
                    ShiftTile(i, j);
                }
            }
        }
    }

    void ShiftTile(int nullIndexX, int nullIndexY)
    {
        for (int i = nullIndexY + 1; i < gridSize; i++)
        {
            if (tiles[nullIndexX, i] != null)
            {
                tiles[nullIndexX, nullIndexY] = tiles[nullIndexX, i];
                //Move(tiles[nullIndexX, nullIndexY], new Vector3(tiles[nullIndexX, nullIndexY].transform.position.x, nullIndexY + offset.transform.position.y, offset.transform.position.z));
                tiles[nullIndexX, i] = null;
                StartCoroutine(ShiftAnimation(tiles[nullIndexX, nullIndexY], new Vector3(tiles[nullIndexX, nullIndexY].transform.position.x, nullIndexY + offset.transform.position.y, offset.transform.position.z)));
                return;
            }
        }
        for (int i = gridSize; i < gridSize * 2; i++)
        {
            if (extendedTiles[nullIndexX, i] != null)
            {
                tiles[nullIndexX, nullIndexY] = extendedTiles[nullIndexX, i];
                //Move(tiles[nullIndexX, nullIndexY], new Vector3(tiles[nullIndexX, nullIndexY].transform.position.x, nullIndexY + offset.transform.position.y, offset.transform.position.z));
                extendedTiles[nullIndexX, i] = null;
                StartCoroutine(ShiftAnimation(tiles[nullIndexX, nullIndexY], new Vector3(tiles[nullIndexX, nullIndexY].transform.position.x, nullIndexY + offset.transform.position.y, offset.transform.position.z)));
                return;
            }
        }
    }

    //For testing purposes
    void Move(GameObject tile, Vector3 endPos)
    {
        tile.transform.position = endPos;
    }

    IEnumerator ShiftAnimation(GameObject tile, Vector3 endPos)
    {
        float normalizedTime = 0f;


        while (normalizedTime <= duration)
        {
            normalizedTime += Time.deltaTime / duration;

            tile.transform.position = Vector3.Lerp(tile.transform.position, endPos, normalizedTime);
            yield return null;
        }
    }
}