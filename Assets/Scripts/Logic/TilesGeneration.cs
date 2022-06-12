using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TilesGeneration : MonoBehaviour
{
    [SerializeField] GameObject[] tilesPrefabs;  //Test tile prefab for first board filling
    [SerializeField] float duration = 1f; //Duration of shifting
    [SerializeField] TilesField tilesField;

    Chain chain;

    GameObject offset; //Tiles matrix offset
    readonly System.Random rnd = new();

    private void Start()
    {
        chain = GetComponent<Chain>();
    }

    public void FirstGenerate()
    {
        offset = GameObject.Find("TileOffset");

        GameObject[] generatedTiles = GetRandomPrefabs(TilesField.gridSize * TilesField.gridSize, 3, 3);
        int tileIndex = 0;
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
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
                tilesField.tiles[i, j] = newTile;
            }
        }
    }

    GameObject[] GetRandomPrefabs(int prefabsCount, int minEnemyNumber, int maxEnemyNumber)
    {
        // generate prefabs randomly with enemies >= min number and <= max number
        GameObject[] prefabs = tilesPrefabs; //prefabs from which to generate
        int enemyNumber = 0; //how many enemies was generated

        //shuffled list with numbers from 0 to prefabsCount count
        int[] shuffledPositions = Enumerable.Range(0, prefabsCount)
            .ToList()
            .OrderBy(a => rnd.Next()).ToArray();

        // array with tiles to add. Now is empty
        GameObject[] generatedTiles = new GameObject[prefabsCount];

        // change this in the future when adding elite enemies
        GameObject regularEnemyPrefab = prefabs.Where(val => val.GetComponent<TileClass>().tileName == TileNameE.RegularEnemy).First();
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

    public void GenereteNewTilesAfterChain(int[] numToGen)
    {
        int maxEnemyNumber = Mathf.CeilToInt((float)chain.chain.Count / 2); // max enemies to generate
        int minEnemyNumber = 1; //min enemies to generate

        // generate tiles
        GameObject[] generatedTiles = GetRandomPrefabs(chain.chain.Count, minEnemyNumber, maxEnemyNumber);

        //instantiate tiles
        int tileIndex = 0;
        for (int i = 0; i < TilesField.gridSize; i++)
        {
            for (int j = 0; j < numToGen[i]; j++)
            {
                tilesField.extendedTiles[i, j] = Instantiate(
                    generatedTiles[tileIndex],
                    new Vector3(offset.transform.position.x + i,
                    offset.transform.position.y + TilesField.gridSize + j,
                    offset.transform.position.z),
                    Quaternion.identity, offset.transform);
                tilesField.extendedTiles[i, j].name = "[" + (j + 1).ToString() + "]" + "[" + (i + 1).ToString() + "] TMP";
                tileIndex++;
            }
        }

        //shift tiles to empty spaces
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                if (tilesField.tiles[i, j] == null)
                {
                    ShiftTile(i, j);
                }
            }
        }
    }

    void ShiftTile(int nullIndexX, int nullIndexY)
    {
        for (int i = nullIndexY + 1; i < TilesField.gridSize; i++)
        {
            if (tilesField.tiles[nullIndexX, i] != null)
            {
                tilesField.tiles[nullIndexX, nullIndexY] = tilesField.tiles[nullIndexX, i];
                //Move(tiles[nullIndexX, nullIndexY], new Vector3(tiles[nullIndexX, nullIndexY].transform.position.x, nullIndexY + offset.transform.position.y, offset.transform.position.z));
                tilesField.tiles[nullIndexX, i] = null;
                StartCoroutine(
                    ShiftAnimation(
                        tilesField.tiles[nullIndexX, nullIndexY], 
                        new Vector3(
                            tilesField.tiles[nullIndexX, nullIndexY].transform.position.x,
                            nullIndexY + offset.transform.position.y,
                            offset.transform.position.z)));
                return;
            }
        }
        for (int i = 0; i < TilesField.gridSize; i++)
        {
            if (tilesField.extendedTiles[nullIndexX, i] != null)
            {
                tilesField.tiles[nullIndexX, nullIndexY] = tilesField.extendedTiles[nullIndexX, i];
                //Move(tiles[nullIndexX, nullIndexY], new Vector3(tiles[nullIndexX, nullIndexY].transform.position.x, nullIndexY + offset.transform.position.y, offset.transform.position.z));
                tilesField.extendedTiles[nullIndexX, i] = null;
                StartCoroutine(ShiftAnimation(
                    tilesField.tiles[nullIndexX, nullIndexY],
                    new Vector3(
                        tilesField.tiles[nullIndexX, nullIndexY].transform.position.x,
                        nullIndexY + offset.transform.position.y,
                        offset.transform.position.z)));
                return;
            }
        }
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

    //For testing purposes
    void Move(GameObject tile, Vector3 endPos)
    {
        tile.transform.position = endPos;
    }
}
