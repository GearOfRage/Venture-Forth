using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject testTile;  //Test tile prefab for first board filling
    public GameObject testTile2; //Test tile prefab for generation
    public GameObject tempTile;  //NULL tile prefab

    public GameObject[,] tiles; //Tiles matrix
    GameObject offset; //Tiles matrix offset

    static int gridSize = 6; //Size of both dimentions
    static int minChainSize = 3; // Minimal chain size

    public bool isDragStarted = false; //Boolean for detection of draging the chain
    public bool IsShifting = false; //Boolean for detection of starting shift down (unuseble)
    float interpolationAmmount; //
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
                GenereteNewTile();
            }

            //Some game logic happens <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        }

        chainRenderer.DestroyNodes();
        chain.Clear(); //Clearing the chain elements
    }

    void GenereteNewTile()
    {
        int[] numToGen = new int[gridSize];
        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                if (chain.Contains(tiles[i, j]))
                {
                    //numToGen[i]++;
                    GameObject.Destroy(tiles[i, j]);
                    tiles[i, j] = Instantiate(testTile2, new Vector3(tiles[i, j].transform.position.x, gridSize + j, offset.transform.position.z), Quaternion.identity, offset.transform);
                    tiles[i, j].name = "[" + (j + 1).ToString() + "]" + "[" + (i + 1).ToString() + "] TMP";
                    ShiftTile(i, j);
                }
            }
        }
    }

    void ShiftTile(int nullIndexX, int nullIndexY)
    {
        for (int i = nullIndexY; i < gridSize; i++)
        {
            if (!tiles[nullIndexX, i].CompareTag("tmp"))
            {
                Debug.Log("Swaped:" + tiles[nullIndexX, i] + " : " + tiles[nullIndexX, nullIndexY]);

                GameObject tmp;
                tmp = tiles[nullIndexX, i];
                tiles[nullIndexX, i] = tiles[nullIndexX, nullIndexY];
                tiles[nullIndexX, nullIndexY] = tmp;

                StartCoroutine(ShiftAnimation(tiles[nullIndexX, i], tiles[nullIndexX, nullIndexY]));
            }
        }
    }

    IEnumerator ShiftAnimation(GameObject tileA, GameObject TileB)
    {
        float normalizedTime = 0;

        Vector3 posA = tileA.transform.position;
        Vector3 posB = TileB.transform.position;

        while (normalizedTime <= duration)
        {
            normalizedTime = Time.deltaTime / duration;

            tileA.transform.position = Vector3.Lerp(tileA.transform.position, posB, (interpolationAmmount + normalizedTime));
            TileB.transform.position = Vector3.Lerp(TileB.transform.position, posA, (interpolationAmmount + normalizedTime));

            yield return null;
        }
    }
}