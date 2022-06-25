using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

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
    [SerializeField] GameLogic gl;
    Chain chain;
    [SerializeField] ProgressLogic pl;
    ChainBehaviour cb;

    [Inject]
    DamageService damageService;
    [Inject]
    TilesField tilesField;

    public bool isPanelOpen = false;

    public static Action OnTurnEnd;
    public static Action<ProgressTypeE> OnCollect;

    private void Start()
    {
        turnText.text = gl.gameStats.turnNumber.ToString();
        cb = GameObject.Find("GameManager").GetComponent<ChainBehaviour>();
        chain = GetComponent<Chain>();
    }

    public void Next()
    {
        HandleChain();
        if (isPanelOpen)
        {
            return;
        }
        TurnEnd();
    }

    public void TurnEnd()
    {
        // no regeneration for 0 hp!
        CalculateDamageToPlayer();

        cb.OnPanelClose();

        gl.CheckGameOver();
        gl.gameStats.turnNumber++;
        turnText.text = gl.gameStats.turnNumber.ToString();
        OnTurnEnd?.Invoke();
    }

    int GetDmgToEnemyByTileName(TileNameE tileName)
    {
        return tileName switch
        {
            TileNameE.RegularEnemy => 0,
            TileNameE.Sword => gl.player.weaponDamage,
            TileNameE.MagicSword => gl.player.weaponDamage * 5,
            _ => throw new Exception("Unexpected attack " + tileName),
        };
    }

    public void CalculatePotentialDamageToEnemies()
    {
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                GameObject tile = tilesField.tiles[i, j];
                if (tile == null)
                {
                    continue;
                }

                if (tile.TryGetComponent<EnemyClass>(out var enemy))
                {
                    enemy.killMark.SetActive(false);
                }
            }
        }

        TileTypeE chainType = chain.chain[0].GetComponent<TileClass>().tileType;
        switch (chainType)
        {
            case TileTypeE.Attack:
                int dmgAmount = gl.player.baseDamage;
                foreach (GameObject item in chain.chain)
                {
                    dmgAmount += GetDmgToEnemyByTileName(item.GetComponent<TileClass>().tileName);
                }
                foreach (GameObject item in chain.chain)
                {
                    TileNameE tileName = item.GetComponent<TileClass>().tileName;
                    EnemyClass enemy = item.GetComponent<EnemyClass>();
                    switch (tileName)
                    {
                        case TileNameE.RegularEnemy:
                            HpArmourS hpArmour = damageService.CalculateDamageWithArmour(
                                dmgAmount, enemy.armour, enemy.hp,
                                0.1f
                             );
                            if (hpArmour.hp <= 0)
                            {
                                enemy.killMark.SetActive(true);
                            }
                            break;
                        case TileNameE.Sword:
                            break;
                        default:
                            throw new Exception("Unexpected attack " + tileName);
                    }
                }
                break;
            default:
                break;
        }
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
                    dmgAmount += GetDmgToEnemyByTileName(item.GetComponent<TileClass>().tileName);
                }
                foreach (GameObject item in chain.chain)
                {
                    TileNameE tileName = item.GetComponent<TileClass>().tileName;
                    EnemyClass enemy = item.GetComponent<EnemyClass>();
                    switch (tileName)
                    {
                        case TileNameE.RegularEnemy:
                            HpArmourS hpArmour = damageService.CalculateDamageWithArmour(
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
                    //Put Particle system here
                    //OnCollect(ProgressTypeE.Experience);
                }
                int playerLvlUps = expProgressCurrent / gl.player.experienceProgressMax;
                if (playerLvlUps > 0)
                {
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

                //Put Particle system here
                //OnCollect(ProgressTypeE.Equipment);

                int equipmentLevelUps = equipmentProgressCurrent / gl.player.equipmentProgressMax;
                if (equipmentLevelUps > 0)
                {
                    //Debug.Log("Up " + equipmentLevelUps + " equipements now!");
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

                //Put Particle system here
                //OnCollect(ProgressTypeE.Gold);

                int goldLevelUps = goldProgressCurrent / gl.player.coinProgressMax;
                if (goldLevelUps > 0)
                {
                    pl.ShowProgressPanel(ProgressTypeE.Gold, goldLevelUps);
                }
                gl.player.coinProgressCurrent = goldProgressCurrent % gl.player.coinProgressMax;
                break;
            default:
                break;
        }
    }



    public void CalculateDamageToPlayer()
    {
        HpArmourS newHpArmour = damageService.CalculatePlayerHpChange();
        int receivedDamage = gl.player.hpCurrent - newHpArmour.hp;
        gl.gameStats.receivedDamage += receivedDamage;
        gl.player.hpCurrent = newHpArmour.hp;
        gl.player.armourCurrent = newHpArmour.armour;
    }


}
