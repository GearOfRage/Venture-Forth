using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TouchOfWarSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.TouchOfWar;
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

        //Actual spell effect

        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                if (tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileName == TileNameE.Coin)
                {
                    GameObject newTile = Instantiate(
                        tilePrefab, 
                        tg.tilesField.tiles[i, j].transform.position,
                        Quaternion.identity,
                        tg.tilesField.tiles[i, j].transform.parent);
                    Destroy(tg.tilesField.tiles[i, j]);
                    tg.tilesField.tiles[i, j] = newTile;
                }
            }
        }
    }
}
