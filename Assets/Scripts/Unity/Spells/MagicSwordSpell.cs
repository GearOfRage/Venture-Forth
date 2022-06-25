using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class MagicSwordSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.MagicSword;
    [SerializeField] GameObject tilePrefab;

    [Inject]
    TilesGeneration tg;
    void Start()
    {
        //add subscription to spellclass cast event
        SpellClass.OnCast += Cast;
    }

    void Cast(SpellClass castedSpell)
    {
        if (castedSpell.spellName != mySpellName)
        {
            return;
        }

        GameObject[,] tempArray = new GameObject[TilesField.gridSize, TilesField.gridSize];
        int counter = 0;

        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                if (tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileName == TileNameE.Sword)
                {
                    tempArray[i, j] = tg.tilesField.tiles[i, j];
                    counter++;
                }
            }
        }

        if (counter == 0)
        {
            return;
        }

        int rand = Random.Range(0, counter);

        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                if (tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileName == TileNameE.Sword)
                {
                    rand--;
                }
                if (rand <= 0)
                {
                    GameObject newTile = Instantiate(
                        tilePrefab,
                        tg.tilesField.tiles[i, j].transform.position,
                        Quaternion.identity,
                        tg.tilesField.tiles[i, j].transform.parent);
                    Destroy(tg.tilesField.tiles[i, j]);
                    tg.tilesField.tiles[i, j] = newTile;
                    return;
                }
            }
        }
    }
}
