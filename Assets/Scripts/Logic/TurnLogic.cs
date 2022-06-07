using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct HpArmour
{
    public HpArmour(int hp, int armour)
    {
        this.hp = hp;
        this.armour = armour;
    }

    public int hp { get; set; }
    public int armour { get; set; }
}

public class TurnLogic : MonoBehaviour
{
    [SerializeField] Text turnText;
    [SerializeField] PlayerClass player;
    [SerializeField] CollectVisuals collectVisuals;
    [SerializeField] GameLogic gl;
    [SerializeField] ProgressLogic pl;

    int turnNumber = 1;

    private void Start()
    {
        turnText.text = turnNumber.ToString();
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
                int dmgAmount = player.baseDamage;
                int expGain = 0;
                int killedEnemiesCount = 0;
                foreach (GameObject item in gl.chain)
                {
                    TileName tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileName.RegularEnemy:
                            break;
                        case TileName.Sword:
                            dmgAmount += player.weaponDamage;
                            break;
                        default:
                            throw new System.Exception("Unexpected attack " + tileName);
                    }
                }
                foreach (GameObject item in gl.chain)
                {
                    TileName tileName = item.GetComponent<TileClass>().tileName;
                    EnemyClass enemy = item.GetComponent<EnemyClass>();
                    switch (tileName)
                    {
                        case TileName.RegularEnemy:
                            HpArmour hpArmour = CalculateDamageWithArmour(
                                dmgAmount, enemy.armour, enemy.hpMax, enemy.hp,
                                0.1f
                             );
                            enemy.hp = hpArmour.hp;
                            enemy.armour = hpArmour.armour;
                            if (enemy.hp <= 0)
                            {
                                expGain += enemy.experienceGain;
                                killedEnemiesCount++;
                            }
                            else
                            {
                                enemy.UpdateStats();
                            }
                            break;
                        case TileName.Sword:
                            break;
                        default:
                            throw new System.Exception("Unexpected attack " + tileName);
                    }
                }
                expGain += Mathf.FloorToInt(killedEnemiesCount * player.addictionalExperienceProgressByEnemy);
                int expProgressCurrent = player.experienceProgressCurrent + expGain;
                if (expGain > 0)
                {
                    collectVisuals.RunParticles(CollectParticle.ExpParticles);
                }
                int playerLvlUps = expProgressCurrent / player.experienceProgressMax;
                if (playerLvlUps > 0)
                {
                    Debug.Log("Up " + playerLvlUps + " player levels now!");
                    player.characterExpLevel += playerLvlUps;
                    pl.ShowProgressPanel(ProgressType.Experience, playerLvlUps);
                }
                player.experienceProgressCurrent = expProgressCurrent % player.experienceProgressMax;
                break;
            case TileType.Armour:
                int armourGain = 0;
                int shieldCount = 0;
                int equipmentProgressGain = 0;
                foreach (GameObject item in gl.chain)
                {
                    TileName tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileName.Shield:
                            equipmentProgressGain++;
                            armourGain++;
                            shieldCount++;
                            break;
                        default:
                            throw new System.Exception("Unexpected armour " + tileName);
                    }
                }
                equipmentProgressGain += Mathf.FloorToInt(shieldCount * player.addictionalEquipementProgressByShield);
                int equipmentProgressCurrent = player.equipmentProgressCurrent + equipmentProgressGain;
                collectVisuals.RunParticles(CollectParticle.EquipParticles);
                int equipmentLevelUps = equipmentProgressCurrent / player.equipmentProgressMax;
                if (equipmentLevelUps > 0)
                {
                    Debug.Log("Up " + equipmentLevelUps + " equipements now!");
                }
                player.equipmentProgressCurrent = equipmentProgressCurrent % player.equipmentProgressMax;
                player.armourCurrent = Mathf.Min(player.armourMax, armourGain);
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
                int goldCount = 0;
                foreach (GameObject item in gl.chain)
                {
                    TileName tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileName.Coin:
                            goldGain++;
                            goldCount++;
                            break;
                        case TileName.Crown:
                            goldGain += 10;
                            goldCount++;
                            break;
                        default:
                            throw new System.Exception("Unexpected coin " + tileName);
                    }
                }
                goldGain += Mathf.FloorToInt(goldCount * player.addictionalCoinProgressByCoin);
                int goldProgressCurrent = player.coinProgressCurrent + goldGain;
                collectVisuals.RunParticles(CollectParticle.CoinParticles);
                int goldLevelUps = goldProgressCurrent / player.coinProgressMax;
                if (goldLevelUps > 0)
                {
                    pl.ShowProgressPanel(ProgressType.Gold, goldLevelUps);
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
                if (enemy != null && enemy.hp > 0)
                {
                    dmgToPlayer += enemy.attack;
                }
            }
        }


        HpArmour hpArmour = CalculateDamageWithArmour(
            dmgToPlayer,
            player.armourCurrent,
            player.hpMax,
            player.hpCurrent,
            player.damageReductionByArmour
        );
        player.hpCurrent = hpArmour.hp;
        player.armourCurrent = hpArmour.armour;

    }

    HpArmour CalculateDamageWithArmour(int dmg, int armour, int maxHp, int hp, float dmgReductionByArmour)
    {
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
        HpArmour res = new HpArmour(hp, armour);
        if (dmg == 0)
        {
            return res;
        }

        if (armour == 0)
        {
            res.hp = hp - dmg;
            return res;
        }

        float potentialDmgToArmour = dmg * dmgReductionByArmour;
        int dmgReduction = Mathf.Max(1, Mathf.Min(armour, Mathf.RoundToInt(potentialDmgToArmour)));
        int finalDmg = dmg - dmgReduction;
        res.hp = hp - finalDmg;
        res.armour = armour - Mathf.Max(Mathf.Min(armour, Mathf.FloorToInt(potentialDmgToArmour)), 1);

        return res;
    }


}
