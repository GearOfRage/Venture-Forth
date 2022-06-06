using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnLogic : MonoBehaviour
{
    [SerializeField]
    Text turnText;

    [SerializeField]
    PlayerClass player;

    GameLogic gl;
    int turnNumber = 1;

    private void Start()
    {
        turnText.text = turnNumber.ToString();
        gl = GetComponent<GameLogic>();
    }

    public void Next()
    {
        //Some game logic happens <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        HandleChain();
        // no regeneration for 0 hp!
        CalculateDamageToPlayer();

        gl.CheckGameOver();
        player.UpdateBars();
        turnNumber++;
        turnText.text = turnNumber.ToString();
    }

    void HandleChain()
    {
        TileType chainType = gl.chain[0].GetComponent<TileClass>().tileType;
        switch (chainType)
        {
            case TileType.Attack:
                break;
            case TileType.Armour:
                int armourGain = 0;
                int equipmentProgressGain = 0;
                foreach (GameObject item in gl.chain)
                {
                    TileName tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileName.Shield:
                            equipmentProgressGain++;
                            armourGain++;
                            break;
                        default:
                            throw new System.Exception("Unexpected armour " + tileName);
                    }
                }
                equipmentProgressGain += Mathf.FloorToInt(equipmentProgressGain * player.addictionalEquipementProgressByShield);
                int equipmentProgressCurrent = player.equipmentProgressCurrent + equipmentProgressGain;
                int equipmentLevelUps = equipmentProgressCurrent / player.equipmentProgressMax;
                if (equipmentLevelUps > 0)
                {
                    Debug.Log("Up " + equipmentLevelUps + " equipements now!");
                }
                player.equipmentProgressCurrent = equipmentProgressCurrent % player.equipmentProgressMax;
                player.armourCurrent = Mathf.Max(player.armourMax, armourGain);
                break;
            case TileType.Potion:
                int healthChange = 0;
                foreach (GameObject item in gl.chain)
                {
                    TileName tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileName.ExperiencePotion:
                            player.experienceProgressCurrent += 10;
                            break;
                        case TileName.PosionPoition:
                            healthChange -= player.hpByPotion;
                            break;
                        case TileName.HealthPotion:
                            healthChange += player.hpByPotion;
                            break;
                        default:
                            throw new System.Exception("Unexpected potion " + tileName);
                    }
                }
                player.hpCurrent = Mathf.Clamp(player.hpCurrent + healthChange, 0, player.hpMax);
                break;
            case TileType.Gold:
                int goldGain = 0;
                foreach (GameObject item in gl.chain)
                {
                    TileName tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileName.Coin:
                             goldGain++;
                            break;
                        case TileName.Crown:
                            goldGain += 10;
                            break;
                        default:
                            throw new System.Exception("Unexpected coin " + tileName);
                    }
                }
                goldGain += Mathf.FloorToInt(goldGain * player.addictionalCoinProgressByCoin);
                int goldProgressCurrent = player.coinProgressCurrent + goldGain;
                int goldLevelUps = goldProgressCurrent / player.coinProgressMax;
                if (goldLevelUps > 0)
                {
                    Debug.Log("Up " + goldLevelUps + " golds now!");
                }
                player.coinProgressCurrent = goldProgressCurrent % player.coinProgressMax;
                break;
            default:
                break;
        }
    }

    void CalculateDamageToPlayer()
    {
        int dmgToPlayer = 0;
        for (int i = 0; i < GameLogic.gridSize; i++) //Columns
        {
            for (int j = 0; j < GameLogic.gridSize; j++) //Rows
            {
                GameObject tile = gl.tiles[i, j];
                if (tile == null)
                {
                    continue;
                }

                EnemyClass enemy = tile.GetComponent<EnemyClass>();
                if (enemy != null)
                {
                    dmgToPlayer += enemy.attack;
                }
            }
        }
        if (dmgToPlayer == 0)
        {
            return;
        }

        /**
         * Armour logic explained:
         * case 1
         * player has 1 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 27
         * potentialDmgToArmour = 27 * 0.1 = 2.7
         * dmg to armour = Min(1 (currentArmour), Round(2.7), 1) = 1;
         * damagedArmour = Min(1 (currentArmour), Floor(2.7), 1) = 1
         * player now has 1 - 1 armour = 0, 50 - 27 + 1 = 24
         * 
         * case 2
         * player has 3 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 17
         * potentialDmgToArmour = 17 * 0.1 = 1.7
         * dmg to armour = Min(3 (currentArmour), Round(1.7), 1) = 2;
         * damagedArmour = Min(1 (currentArmour), Floor(1.7), 1) = 1
         * player now has 3 - 1 armour = 2, 50 - 17 + 2 = 35
        */

        if (player.armourCurrent == 0)
        {
            player.hpCurrent -= dmgToPlayer;
        }
        else
        {
            float potentialDmgToArmour = dmgToPlayer * player.damageReductionByArmour;
            int dmgReduction = Mathf.Max(1, Mathf.Min(player.armourCurrent, Mathf.RoundToInt(potentialDmgToArmour)));
            int finalDmgToPlayer = dmgToPlayer - dmgReduction;
            player.armourCurrent -= Mathf.Max(Mathf.Min(player.armourCurrent, Mathf.FloorToInt(potentialDmgToArmour)), 1);
            player.hpCurrent -= finalDmgToPlayer;
        }

    }


}
