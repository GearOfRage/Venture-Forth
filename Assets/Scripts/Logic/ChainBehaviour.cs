using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBehaviour : MonoBehaviour
{
    public ChainRenderer chainRenderer; //ChainRenderer script

    Chain chain;
    TilesField tilesField;
    TilesGeneration tg;

    public bool isDragStarted;

    void Start()
    {
        isDragStarted = false;
        chainRenderer = gameObject.GetComponentInChildren<ChainRenderer>();
        chain = GetComponent<Chain>();
        tilesField = GetComponent<TilesField>();
        tg = GetComponent<TilesGeneration>();
    }
    public void ChainDone()
    {
        if (chain.chain.Count >= chain.minChainSize)
        {
            // first handle chain
            TurnLogic tl = GetComponent<TurnLogic>();
            tl.Next();
            // then destroy it
            int[] numToGen = CalculateAndDestroyChainTiles();
            // then generate new tiles
            tg.GenereteNewTilesAfterChain(numToGen);
        }

        chainRenderer.DestroyNodes();
        chain.chain.Clear(); //Clearing the chain elements
    }
    int[] CalculateAndDestroyChainTiles()
    {
        int[] numToGen = new int[TilesField.gridSize];

        // count how many tiles to generate in each column
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                int index = chain.chain.IndexOf(tilesField.tiles[i, j]);
                if (index != -1)
                {
                    EnemyClass e = tilesField.tiles[i, j].GetComponent<EnemyClass>();
                    if (e == null || e.hp <= 0)
                    {
                        numToGen[i]++;
                        Destroy(tilesField.tiles[i, j]);
                        tilesField.tiles[i, j] = null;
                    }
                }
            }
        }

        return numToGen;
    }

}
