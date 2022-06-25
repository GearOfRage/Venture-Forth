using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.Break;
    [SerializeField] GameObject tilePrefab;

    TilesGeneration tg;
    void Start()
    {
        tg = GameObject.Find("GameManager").GetComponent<TilesGeneration>();
        //add subscription to spellclass cast event
        SpellClass.OnCast += Cast;
    }

    void Cast(SpellClass castedSpell)
    {
        if (castedSpell.spellName != mySpellName)
        {
            return;
        }

        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                if (tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileName == TileNameE.RegularEnemy ||
                    tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileName == TileNameE.EliteEnemy)
                {
                    tg.tilesField.tiles[i, j].GetComponent<EnemyClass>().armour = 0;
                    tg.tilesField.tiles[i, j].GetComponent<EnemyClass>().armourText.text = "0";
                }
            }
        }
    }
}
