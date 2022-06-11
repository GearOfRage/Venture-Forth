using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct HpArmourS
{
    public HpArmourS(int hp, int armour)
    {
        this.hp = hp;
        this.armour = armour;
    }

    public int hp { get; set; }
    public int armour { get; set; }
}

public class TurnLogic : MonoBehaviour
{
    [SerializeField] public Text turnText;
    [SerializeField] CollectVisuals collectVisuals;
    [SerializeField] TilesField tilesField;
    [SerializeField] GameLogic gl;
    Chain chain;
    [SerializeField] ProgressLogic pl;

    private void Start()
    {
        turnText.text = gl.gameStats.turnNumber.ToString();
        chain = GetComponent<Chain>();
    }

    public void Next()
    {
        //Some game logic happens <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        HandleChain();
        // no regeneration for 0 hp!
        CalculateDamageToPlayer();

        gl.CheckGameOver();
        gl.player.UpdateBars();
        gl.gameStats.turnNumber++;
        turnText.text = gl.gameStats.turnNumber.ToString();
    }

    void HandleChain()
    {
        TileTypeE chainType = chain.chain[0].GetComponent<TileClass>().tileType;
        switch (chainType)
        {
            case TileTypeE.Attack:
                int dmgAmount = gl.player.baseDamage;
                int expGain = 0;
                int killedEnemiesCount = 0;
                foreach (GameObject item in chain.chain)
                {
                    TileNameE tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileNameE.RegularEnemy:
                            break;
                        case TileNameE.Sword:
                            dmgAmount += gl.player.weaponDamage;
                            break;
                        default:
                            throw new System.Exception("Unexpected attack " + tileName);
                    }
                }
                foreach (GameObject item in chain.chain)
                {
                    TileNameE tileName = item.GetComponent<TileClass>().tileName;
                    EnemyClass enemy = item.GetComponent<EnemyClass>();
                    switch (tileName)
                    {
                        case TileNameE.RegularEnemy:
                            HpArmourS hpArmour = CalculateDamageWithArmour(
                                dmgAmount, enemy.armour, enemy.hp,
                                0.1f
                             );
                            enemy.hp = hpArmour.hp;
                            enemy.armour = hpArmour.armour;
                            if (enemy.hp <= 0)
                            {
                                gl.gameStats.killedRegularEnemies += 1;
                                expGain += enemy.experienceGain;
                                killedEnemiesCount++;
                            }
                            else
                            {
                                enemy.UpdateStats();
                            }
                            break;
                        case TileNameE.Sword:
                            break;
                        default:
                            throw new System.Exception("Unexpected attack " + tileName);
                    }
                }
                expGain += Mathf.FloorToInt(killedEnemiesCount * gl.player.addictionalExperienceProgressByEnemy);
                int expProgressCurrent = gl.player.experienceProgressCurrent + expGain;
                if (expGain > 0)
                {
                    collectVisuals.RunParticles(CollectParticleE.ExpParticles);
                }
                int playerLvlUps = expProgressCurrent / gl.player.experienceProgressMax;
                if (playerLvlUps > 0)
                {
                    Debug.Log("Up " + playerLvlUps + " gl.player levels now!");
                    gl.player.characterExpLevel += playerLvlUps;
                    pl.ShowProgressPanel(ProgressTypeE.Experience, playerLvlUps);
                }
                gl.player.experienceProgressCurrent = expProgressCurrent % gl.player.experienceProgressMax;
                break;
            case TileTypeE.Armour:
                int armourGain = 0;
                int shieldCount = 0;
                int equipmentProgressGain = 0;
                foreach (GameObject item in chain.chain)
                {
                    TileNameE tileName = item.GetComponent<TileClass>().tileName;
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
                }
                equipmentProgressGain += Mathf.FloorToInt(shieldCount * gl.player.addictionalEquipementProgressByShield);
                int equipmentProgressCurrent = gl.player.equipmentProgressCurrent + equipmentProgressGain;
                collectVisuals.RunParticles(CollectParticleE.EquipParticles);
                int equipmentLevelUps = equipmentProgressCurrent / gl.player.equipmentProgressMax;
                if (equipmentLevelUps > 0)
                {
                    Debug.Log("Up " + equipmentLevelUps + " equipements now!");
                }
                gl.player.equipmentProgressCurrent = equipmentProgressCurrent % gl.player.equipmentProgressMax;
                gl.player.armourCurrent = Mathf.Min(gl.player.armourMax, armourGain);
                break;
            case TileTypeE.Potion:
                int healthChange = 0;
                foreach (GameObject item in chain.chain)
                {
                    TileNameE tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileNameE.ExperiencePotion:
                            gl.player.experienceProgressCurrent += 10;
                            break;
                        case TileNameE.PosionPoition:
                            healthChange -= gl.player.hpByPotion;
                            break;
                        case TileNameE.HealthPotion:
                            healthChange += gl.player.hpByPotion;
                            break;
                        default:
                            throw new System.Exception("Unexpected potion " + tileName);
                    }
                }
                gl.player.hpCurrent = Mathf.Clamp(gl.player.hpCurrent + healthChange, 0, gl.player.hpMax);
                break;
            case TileTypeE.Gold:
                int goldGain = 0;
                int goldCount = 0;
                foreach (GameObject item in chain.chain)
                {
                    TileNameE tileName = item.GetComponent<TileClass>().tileName;
                    switch (tileName)
                    {
                        case TileNameE.Coin:
                            goldGain++;
                            goldCount++;
                            break;
                        case TileNameE.Crown:
                            goldGain += 10;
                            goldCount++;
                            break;
                        default:
                            throw new System.Exception("Unexpected coin " + tileName);
                    }
                }
                goldGain += Mathf.FloorToInt(goldCount * gl.player.addictionalCoinProgressByCoin);
                gl.gameStats.collectedGold += goldGain;
                int goldProgressCurrent = gl.player.coinProgressCurrent + goldGain;
                collectVisuals.RunParticles(CollectParticleE.CoinParticles);
                int goldLevelUps = goldProgressCurrent / gl.player.coinProgressMax;
                if (goldLevelUps > 0)
                {
                    pl.ShowProgressPanel(ProgressTypeE.Gold, goldLevelUps);
                    Debug.Log("Up " + goldLevelUps + " golds now!");
                }
                gl.player.coinProgressCurrent = goldProgressCurrent % gl.player.coinProgressMax;
                break;
            default:
                break;
        }
    }

    void CalculateDamageToPlayer()
    {
        int dmgToPlayer = 0;
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {   
                GameObject tile = tilesField.tiles[i, j];
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


        HpArmourS hpArmour = CalculateDamageWithArmour(
            dmgToPlayer,
            gl.player.armourCurrent,
            gl.player.hpCurrent,
            gl.player.damageReductionByArmour
        );
        int receivedDamage = gl.player.hpCurrent - hpArmour.hp;
        gl.gameStats.receivedDamage += receivedDamage;
        gl.player.hpCurrent = hpArmour.hp;
        gl.player.armourCurrent = hpArmour.armour;

    }

    HpArmourS CalculateDamageWithArmour(int dmg, int armour, int hp, float dmgReductionByArmour)
    {
        /**
         * Armour logic explained:
         * case 1
         * gl.player has 1 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 27
         * potentialDmgToArmour = 27 * 0.1 = 2.7
         * dmg to armour = Min(1 (currentArmour), Round(2.7), 1) = 1;
         * damagedArmour = Min(1 (currentArmour), Floor(2.7), 1) = 1
         * gl.player now has 1 - 1 armour = 0, 50 - 27 + 1 = 24
         * 
         * case 2
         * gl.player has 3 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 17
         * potentialDmgToArmour = 17 * 0.1 = 1.7
         * dmg to armour = Min(3 (currentArmour), Round(1.7), 1) = 2;
         * damagedArmour = Min(1 (currentArmour), Floor(1.7), 1) = 1
         * gl.player now has 3 - 1 armour = 2, 50 - 17 + 2 = 35
        */
        HpArmourS res = new(hp, armour);
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
