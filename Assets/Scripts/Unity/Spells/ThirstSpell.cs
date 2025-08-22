using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ThirstSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.Thirst;

    [Inject]
    GameLogic gl;
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

        int[] numToGen = new int[TilesField.gridSize];
        int healthChange = 0;

        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                GameObject tile = tg.tilesField.tiles[i, j];
                if (tile != null && tile.GetComponent<TileClass>().tileType == TileTypeE.Potion)
                {
                    numToGen[i]++;
                    TileNameE tileName = tile.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileNameE.ExperiencePotion:
                            if (gl.player != null)
                                gl.player.experienceProgressCurrent += gl.player.hpByPotion;
                            break;
                        case TileNameE.Poison:
                            if (gl.player != null)
                                healthChange -= gl.player.hpByPotion;
                            break;
                        case TileNameE.HealthPotion:
                            if (gl.player != null)
                                healthChange += gl.player.hpByPotion;
                            break;
                        case TileNameE.ManaPotion:
                            //Decrease random spell CD by hpByPotion
                            break;
                        default:
                            throw new System.Exception("Unexpected potion " + tileName);
                    }

                    Destroy(tile);
                    tg.tilesField.tiles[i, j] = null;
                }
            }
        }

        // Null check to prevent errors if PlayerClass is destroyed
        if (gl.player != null)
        {
            gl.player.hpCurrent = Mathf.Clamp(gl.player.hpCurrent + healthChange, 0, gl.player.hpMax);
        }

        PlayerClass.onStatUpdate?.Invoke();
        PlayerClass.onBarsUpdate?.Invoke();
        tg.GenereteNewTilesAfterChain(numToGen);
    }
}
