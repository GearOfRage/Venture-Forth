using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldsUpSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.ShieldsUp;

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
        int armourGain = 0;
        int shieldCount = 0;
        int equipmentProgressGain = 0;
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                if (tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileType == TileTypeE.Armour)
                {
                    numToGen[i]++;


                    TileNameE tile = tg.tilesField.tiles[i, j].GetComponent<TileClass>().tileName;
                    switch (tile)
                    {
                        case TileNameE.Shield:
                            equipmentProgressGain++;
                            armourGain++;
                            shieldCount++;
                            break;
                        default:
                            throw new System.Exception("Unexpected armour " + tile);
                    }
                    Destroy(tg.tilesField.tiles[i, j]);
                    tg.tilesField.tiles[i, j] = null;
                }
            }
        }
        tg.GenereteNewTilesAfterChain(numToGen);

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

        PlayerClass.onStatUpdate?.Invoke();
        PlayerClass.onBarsUpdate?.Invoke();
    }
}
