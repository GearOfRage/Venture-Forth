using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ShieldsUpSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.ShieldsUp;

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
        int armourGain = 0;
        int shieldCount = 0;
        int equipmentProgressGain = 0;
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                GameObject tile = tg.tilesField.tiles[i, j];
                if (tile != null && tile.GetComponent<TileClass>().tileType == TileTypeE.Armour)
                {
                    numToGen[i]++;

                    TileNameE tileName = tile.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileNameE.Shield:
                            equipmentProgressGain++;
                            armourGain++;
                            shieldCount++;
                            break;
                        default:
                            throw new System.Exception("Unexpected armour " + tileName);
                    }
                    Destroy(tile);
                    tg.tilesField.tiles[i, j] = null;
                }
            }
        }
        tg.GenereteNewTilesAfterChain(numToGen);

        if (gl.player != null)
        {
            equipmentProgressGain += Mathf.FloorToInt(shieldCount * gl.player.addictionalEquipementProgressByShield);
            int equipmentProgressCurrent = gl.player.equipmentProgressCurrent + equipmentProgressGain;

            //Put Particle system here
            TurnLogic.OnCollect(ProgressTypeE.Equipment);

            int equipmentLevelUps = equipmentProgressCurrent / gl.player.equipmentProgressMax;
            if (equipmentLevelUps > 0)
            {
                Debug.Log("Up " + equipmentLevelUps + " equipements now!");
            }
            gl.player.equipmentProgressCurrent = equipmentProgressCurrent % gl.player.equipmentProgressMax;
            gl.player.armourCurrent = Mathf.Min(gl.player.armourMax, armourGain);
        }

        PlayerClass.onStatUpdate?.Invoke();
        PlayerClass.onBarsUpdate?.Invoke();
    }
}
