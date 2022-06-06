using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameLogic : MonoBehaviour
{
    [SerializeField] GameObject[] tilesPrefabs;  //Test tile prefab for first board filling

    public GameObject[,] tiles; //Tiles matrix
    public GameObject[,] extendedTiles; //Extended tiles matrix

    GameObject offset; //Tiles matrix offset

    public static int gridSize = 6; //Size of both dimentions
    static int minChainSize = 3; // Minimal chain size

    public bool isDragStarted = false; //Boolean for detection of draging the chain
    public bool IsShifting = false; //Boolean for detection of starting shift down (unuseble)
    [SerializeField] float duration = 1f; //Duration of shifting

    public List<GameObject> chain; //List for chain elements

    //Visuals
    public ChainRenderer chainRenderer; //ChainRenderer script
    public GameObject node; //Prefab for head and tail of chain
    public SpriteRenderer screenFader;

    [SerializeField]
    PlayerClass player;

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
        extendedTiles = new GameObject[gridSize, gridSize];

        GameObject[] generatedTiles = GetRandomPrefabs(gridSize * gridSize, 3, 3);
        int tileIndex = 0;
        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                GameObject newTile = Instantiate(
                    generatedTiles[tileIndex],
                    new Vector3(
                        offset.transform.position.x + i,
                        offset.transform.position.y + j,
                        offset.transform.position.z
                        ),
                    Quaternion.identity, offset.transform);
                tileIndex++;
                newTile.name = "[" + (j + 1).ToString() + "]" + "[" + (i + 1).ToString() + "]";
                tiles[i, j] = newTile;
            }
        }
    }

    public void ChainDone()
    {
        if (chain.Count >= minChainSize)
        {
            // first handle chain
            TurnLogic tl = GetComponent<TurnLogic>();
            tl.Next();
            // then destroy it
            int[] numToGen = CalculateAndDestroyChainTiles();
            // then generate new tiles
            GenereteNewTiles(numToGen);
        }

        chainRenderer.DestroyNodes();
        chain.Clear(); //Clearing the chain elements
    }

    private System.Random rng = new();

    GameObject[] GetRandomPrefabs(int prefabsCount, int minEnemyNumber, int maxEnemyNumber)
    {
        // generate prefabs randomly with enemies >= min number and <= max number
        GameObject[] prefabs = tilesPrefabs; //prefabs from which to generate
        int enemyNumber = 0; //how many enemies was generated

        //shuffled list with numbers from 0 to prefabsCount count
        int[] shuffledPositions = Enumerable.Range(0, prefabsCount)
            .ToList()
            .OrderBy(a => rng.Next()).ToArray();

        // array with tiles to add. Now is empty
        GameObject[] generatedTiles = new GameObject[prefabsCount];

        // change this in the future when adding elite enemies
        GameObject regularEnemyPrefab = prefabs.Where(val => val.GetComponent<TileClass>().tileName == TileName.RegularEnemy).First();
        // add minimum enemies first
        for (int i = 0; i < minEnemyNumber; i++)
        {
            generatedTiles[shuffledPositions[i]] = regularEnemyPrefab;
            enemyNumber++;
        }
        if (minEnemyNumber == maxEnemyNumber)
        {
            prefabs = prefabs.Where(val => val.GetComponent<EnemyClass>() == null).ToArray();
        }
        // fill all other places randomly
        for (int i = minEnemyNumber; i < prefabsCount; i++)
        {
            generatedTiles[shuffledPositions[i]] = prefabs[Random.Range(0, prefabs.Length)];
            // if max enemies is generated, remove enemies prefab from prefab list
            EnemyClass enemyClass = generatedTiles[shuffledPositions[i]].GetComponent<EnemyClass>();
            // add here all enemies names
            if (enemyClass != null)
            {
                enemyNumber++;
                if (enemyNumber == maxEnemyNumber)
                {
                    prefabs = prefabs.Where(val => val.GetComponent<EnemyClass>() == null).ToArray();
                }
            }
        }
        return generatedTiles;
    }

    int[] CalculateAndDestroyChainTiles()
    {
        int[] numToGen = new int[gridSize];

        // count how many tiles to generate in each column
        for (int i = 0; i < gridSize; i++) //Columns
        {
            for (int j = 0; j < gridSize; j++) //Rows
            {
                int index = chain.IndexOf(tiles[i, j]);
                if (index != -1)
                {
                    EnemyClass e = tiles[i, j].GetComponent<EnemyClass>();
                    if (e == null || e.hp <= 0)
                    {
                        numToGen[i]++;
                        GameObject.Destroy(tiles[i, j]);
                        tiles[i, j] = null;
                    }
                }
            }
        }

        return numToGen;
    }

    void GenereteNewTiles(int[] numToGen)
    {
        int maxEnemyNumber = Mathf.CeilToInt((float)chain.Count / 2); // max enemies to generate
        int minEnemyNumber = 1; //min enemies to generate

        // generate tiles
        GameObject[] generatedTiles = GetRandomPrefabs(chain.Count, minEnemyNumber, maxEnemyNumber);

        //instantiate tiles
        int tileIndex = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < numToGen[i]; j++)
            {
                extendedTiles[i, j] = Instantiate(
                    generatedTiles[tileIndex],
                    new Vector3(offset.transform.position.x + i,
                    offset.transform.position.y + gridSize + j,
                    offset.transform.position.z),
                    Quaternion.identity, offset.transform);
                extendedTiles[i, j].name = "[" + (j + 1).ToString() + "]" + "[" + (i + 1).ToString() + "] TMP";
                tileIndex++;
            }
        }

        //shift tiles to empty spaces
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
        for (int i = 0; i < gridSize; i++)
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

    public void CheckGameOver()
    {
        if (player.hpCurrent <= 0)
        {
            player.hpCurrent = 0;
            Debug.Log("You ded");
            Camera.main.backgroundColor = Color.red;
        }
    }
}