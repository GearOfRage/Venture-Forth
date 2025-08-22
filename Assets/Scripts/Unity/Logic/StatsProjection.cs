using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[System.Serializable]
public struct ProjectedStats
{
    public int projectedHp;
    public int projectedArmor;
    public int projectedGold;
    public int projectedExperience;
    public int projectedExpLevel;
    public int projectedGoldLevel;
    public int projectedEquipmentLevel;

    public int goldLevelUps;
    public int expLevelUps;
    public int equipmentLevelUps;

    public bool hasChanges;

    public ProjectedStats(int hp, int armor, int gold, int exp, int expLevel, int goldLevel, int equipLevel)
    {
        projectedHp = hp;
        projectedArmor = armor;
        projectedGold = gold;
        projectedExperience = exp;
        projectedExpLevel = expLevel;
        projectedGoldLevel = goldLevel;
        projectedEquipmentLevel = equipLevel;

        goldLevelUps = 0;
        expLevelUps = 0;
        equipmentLevelUps = 0;

        hasChanges = false;
    }
}

public class StatsProjection : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public GameObject projectionUI;
    [SerializeField] public Text statsProjectionText;

    [Header("Dependencies")]
    [SerializeField] GameLogic gl;
    [SerializeField] Chain chain;

    [Inject]
    DamageService damageService;

    private ProjectedStats currentProjection;
    private bool isProjectionActive = false;

    void Start()
    {
        if (gl == null)
            gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        if (chain == null)
            chain = GameObject.Find("GameManager").GetComponent<Chain>();

        if (projectionUI != null)
            projectionUI.SetActive(false);
    }

    public void ShowProjection()
    {
        if (chain.chain.Count == 0)
        {
            HideProjection();
            return;
        }

        currentProjection = CalculateProjectedStats();

        if (currentProjection.hasChanges)
        {
            UpdateProjectionUI();
            if (projectionUI != null)
            {
                projectionUI.SetActive(true);
                isProjectionActive = true;
            }
        }
        else
        {
            HideProjection();
        }
    }

    public void HideProjection()
    {
        if (projectionUI != null)
            projectionUI.SetActive(false);
        isProjectionActive = false;
    }

    private ProjectedStats CalculateProjectedStats()
    {
        if (chain.chain.Count == 0)
        {
            return new ProjectedStats(
                gl.player.hpCurrent,
                gl.player.armourCurrent,
                gl.player.coinProgressCurrent,
                gl.player.experienceProgressCurrent,
                gl.player.characterExpLevel,
                gl.player.characterGoldLevel,
                gl.player.characterEqipLevel
            );
        }

        // Start with current stats
        ProjectedStats projection = new ProjectedStats(
            gl.player.hpCurrent,
            gl.player.armourCurrent,
            gl.player.coinProgressCurrent,
            gl.player.experienceProgressCurrent,
            gl.player.characterExpLevel,
            gl.player.characterGoldLevel,
            gl.player.characterEqipLevel
        );

        TileTypeE chainType = chain.chain[0].GetComponent<TileClass>().tileType;

        switch (chainType)
        {
            case TileTypeE.Attack:
                CalculateAttackProjection(ref projection);
                break;
            case TileTypeE.Armour:
                CalculateArmourProjection(ref projection);
                break;
            case TileTypeE.Potion:
                CalculatePotionProjection(ref projection);
                break;
            case TileTypeE.Gold:
                CalculateGoldProjection(ref projection);
                break;
        }

        return projection;
    }

    private void CalculateAttackProjection(ref ProjectedStats projection)
    {
        int dmgAmount = gl.player.baseDamage;
        int expGain = 0;
        int killedEnemiesCount = 0;

        // Calculate total damage
        foreach (GameObject item in chain.chain)
        {
            dmgAmount += GetDmgToEnemyByTileName(item.GetComponent<TileClass>().tileName);
        }

        // Calculate experience gain from killed enemies
        foreach (GameObject item in chain.chain)
        {
            TileNameE tileName = item.GetComponent<TileClass>().tileName;
            if (tileName == TileNameE.RegularEnemy)
            {
                EnemyClass enemy = item.GetComponent<EnemyClass>();
                HpArmourS hpArmour = damageService.CalculateDamageWithArmour(
                    dmgAmount, enemy.armour, enemy.hp, 0.1f
                );

                if (hpArmour.hp <= 0)
                {
                    expGain += enemy.experienceGain;
                    killedEnemiesCount++;
                }
            }
        }

        expGain += Mathf.FloorToInt(killedEnemiesCount * gl.player.addictionalExperienceProgressByEnemy);

        if (expGain > 0)
        {
            projection.hasChanges = true;
            int totalExp = projection.projectedExperience + expGain;
            projection.expLevelUps = totalExp / gl.player.experienceProgressMax;
            projection.projectedExpLevel += projection.expLevelUps;
            projection.projectedExperience = totalExp % gl.player.experienceProgressMax;
        }
    }

    private void CalculateArmourProjection(ref ProjectedStats projection)
    {
        int armourGain = 0;
        int shieldCount = 0;
        int equipmentProgressGain = 0;

        foreach (GameObject item in chain.chain)
        {
            TileNameE tileName = item.GetComponent<TileClass>().tileName;
            if (tileName == TileNameE.Shield)
            {
                equipmentProgressGain++;
                armourGain++;
                shieldCount++;
            }
        }

        equipmentProgressGain += Mathf.FloorToInt(shieldCount * gl.player.addictionalEquipementProgressByShield);

        if (armourGain > 0 || equipmentProgressGain > 0)
        {
            projection.hasChanges = true;
            projection.projectedArmor = Mathf.Min(gl.player.armourMax, gl.player.armourCurrent + armourGain);

            int totalEquipProgress = gl.player.equipmentProgressCurrent + equipmentProgressGain;
            projection.equipmentLevelUps = totalEquipProgress / gl.player.equipmentProgressMax;
            projection.projectedEquipmentLevel += projection.equipmentLevelUps;
        }
    }

    private void CalculatePotionProjection(ref ProjectedStats projection)
    {
        int healthChange = 0;
        int expGain = 0;

        foreach (GameObject item in chain.chain)
        {
            TileNameE tileName = item.GetComponent<TileClass>().tileName;
            switch (tileName)
            {
                case TileNameE.ExperiencePotion:
                    expGain += gl.player.hpByPotion;
                    break;
                case TileNameE.Poison:
                    healthChange -= gl.player.hpByPotion;
                    break;
                case TileNameE.HealthPotion:
                    healthChange += gl.player.hpByPotion;
                    break;
                case TileNameE.ManaPotion:
                    // No immediate stat change
                    break;
            }
        }

        if (healthChange != 0)
        {
            projection.hasChanges = true;
            projection.projectedHp = Mathf.Clamp(projection.projectedHp + healthChange, 0, gl.player.hpMax);
        }

        if (expGain > 0)
        {
            projection.hasChanges = true;
            int totalExp = projection.projectedExperience + expGain;
            projection.expLevelUps = totalExp / gl.player.experienceProgressMax;
            projection.projectedExpLevel += projection.expLevelUps;
            projection.projectedExperience = totalExp % gl.player.experienceProgressMax;
        }
    }

    private void CalculateGoldProjection(ref ProjectedStats projection)
    {
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
            }
        }

        goldGain += Mathf.FloorToInt(goldCount * gl.player.addictionalCoinProgressByCoin);

        if (goldGain > 0)
        {
            projection.hasChanges = true;
            int totalGold = projection.projectedGold + goldGain;
            projection.goldLevelUps = totalGold / gl.player.coinProgressMax;
            projection.projectedGoldLevel += projection.goldLevelUps;
            projection.projectedGold = totalGold % gl.player.coinProgressMax;
        }
    }

    private int GetDmgToEnemyByTileName(TileNameE tileName)
    {
        return tileName switch
        {
            TileNameE.RegularEnemy => 0,
            TileNameE.Sword => gl.player.weaponDamage,
            TileNameE.MagicSword => gl.player.weaponDamage * 5,
            _ => 0,
        };
    }

    private void UpdateProjectionUI()
    {
        if (statsProjectionText == null) return;

        List<string> projectionLines = new List<string>();

        // HP Changes
        int hpChange = currentProjection.projectedHp - gl.player.hpCurrent;
        if (hpChange != 0)
        {
            string sign = hpChange > 0 ? "+" : "";
            string colorTag = hpChange > 0 ? "<color=#C72B1A>" : "<color=#C72B1A>";
            projectionLines.Add($"{colorTag}HP: {sign}{hpChange} → {currentProjection.projectedHp}</color>");
        }

        // Armor Changes
        int armorChange = currentProjection.projectedArmor - gl.player.armourCurrent;
        if (armorChange != 0)
        {
            string sign = armorChange > 0 ? "+" : "";
            string colorTag = armorChange > 0 ? "<color=#0051D0>" : "<color=#C72B1A>";
            projectionLines.Add($"{colorTag}Armor: {sign}{armorChange} → {currentProjection.projectedArmor}</color>");
        }

        // Gold Changes
        int goldChange = currentProjection.projectedGold - gl.player.coinProgressCurrent;
        if (goldChange != 0)
        {
            projectionLines.Add($"<color=#E3860D>Gold: +{goldChange} → {currentProjection.projectedGold}/{gl.player.coinProgressMax}</color>");
        }

        // Experience Changes
        int expChange = currentProjection.projectedExperience - gl.player.experienceProgressCurrent;
        if (expChange != 0)
        {
            projectionLines.Add($"<color=#0CDF36>EXP: +{expChange} → {currentProjection.projectedExperience}/{gl.player.experienceProgressMax}</color>");
        }

        // Level Up Indicators
        if (currentProjection.expLevelUps > 0)
            projectionLines.Add($"<color=#977B40>LEVEL UP! +{currentProjection.expLevelUps}</color>");
        if (currentProjection.goldLevelUps > 0)
            projectionLines.Add($"<color=#977B40>GOLD LEVEL UP! +{currentProjection.goldLevelUps}</color>");
        if (currentProjection.equipmentLevelUps > 0)
            projectionLines.Add($"<color=#977B40>EQUIPMENT LEVEL UP! +{currentProjection.equipmentLevelUps}</color>");

        // Set the final text as single line
        statsProjectionText.text = string.Join(" | ", projectionLines);
    }
}
