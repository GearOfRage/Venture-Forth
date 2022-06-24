using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.Thirst;

    GameLogic gl;
    TilesGeneration tg;
    void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
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

        int[] numToGen = new int[TilesField.gridSize];
        int healthChange = 0;

        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                if (tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileType == TileTypeE.Potion)
                {
                    numToGen[i]++;
                    TileNameE tile = tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileName;
                    switch (tile)
                    {
                        case TileNameE.ExperiencePotion:
                            gl.player.experienceProgressCurrent += gl.player.hpByPotion;
                            break;
                        case TileNameE.Poison:
                            healthChange -= gl.player.hpByPotion;
                            break;
                        case TileNameE.HealthPotion:
                            healthChange += gl.player.hpByPotion;
                            break;
                        case TileNameE.ManaPotion:
                            //Decrease random spell CD by hpByPotion
                            break;
                        default:
                            throw new System.Exception("Unexpected potion " + tile);
                    }

                    Destroy(tg.tilesField.tiles[i, j]);
                    tg.tilesField.tiles[i, j] = null;
                }
            }
        }
        gl.player.hpCurrent = Mathf.Clamp(gl.player.hpCurrent + healthChange, 0, gl.player.hpMax);
        tg.GenereteNewTilesAfterChain(numToGen);
    }
}
