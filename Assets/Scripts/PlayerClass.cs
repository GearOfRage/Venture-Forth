using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterClass
{
    Brawler = 1,
    TreasureHunter = 2,
    Sorcerer = 3
}

public struct Equipement
{
    public EquipItemClass chestSlot;
    public EquipItemClass headSlot;
    public EquipItemClass weaponSlot;
    public EquipItemClass itemSlot;
}

public class PlayerClass : MonoBehaviour
{
    public CharacterClass characterClass;

    [Header("Stats")]

    //Stats
    public int characterEqipLevel = 0;
    public int characterGoldLevel = 0;
    public int characterExpLevel = 1;
    public int hpMax = 20; //Basic stat
    public int hpCurrent = 20;
    public int armourMax = 4; //Basic stat
    public int armourCurrent = 0;
    public int baseDamage = 2; //Basic stat
    public int weaponDamage = 2;
    public int coinProgressMax = 100;
    public int equipmentProgressMax = 100;
    public int experienceProgressMax = 100;
    public int coinProgressCurrent = 0;
    public int equipmentProgressCurrent = 0;
    public int experienceProgressCurrent = 0;
    public int spikes = 0;
    public int hpRegeneration = 0;
    public int vampirism = 0;
    public int hpByPotion = 1;
    public int armourByShield = 1;
    public float addictionalExperienceProgressByEnemy = 0;
    public float addictionalCoinProgressByCoin = 0;
    public float addictionalEquipementProgressByShield = 0;
    public float damageReductionByArmour = 0.1f; //Basic stat

    [Header("Components")]

    //Level and stat
    public Text statsText;
    public Text levelText;

    //Class image
    public SpriteRenderer classIcon;

    //Bars images
    public Image healthBar;
    public Image armourBar;
    public Image coinProgressBar;
    public Image equipmentProgressBar;
    public Image experienceProgressBar;

    //Bars text
    public Text healthBarUpperText;
    public Text armourBarUpperText;
    public Text healthBarLowerText;
    public Text armourBarLowerText;
    public Text coinProgressBarText;
    public Text equipmentProgressBarText;
    public Text experienceProgressBarText;

    //Equipment
    public GameObject chestSlot;
    public GameObject headSlot;
    public GameObject weaponSlot;
    public GameObject itemSlot;
    public Equipement equipement = new();

    //Spells gameobjects
    public GameObject[] spells;

    //Spells text
    public Text[] spellsText;

    private void Start()
    {
        //Getting text component for level and stats display
        statsText = GameObject.Find("StatsUIText").GetComponent<Text>();
        levelText = GameObject.Find("ClassUILevelText").GetComponent<Text>();

        //Getting sprite renderer component for class icon
        classIcon = GameObject.Find("ClassUISprite").GetComponent<SpriteRenderer>();

        //Getting image components for bars
        healthBar = GameObject.Find("HealthBarMeter").GetComponent<Image>();
        armourBar = GameObject.Find("ArmourBarMeter").GetComponent<Image>();
        coinProgressBar = GameObject.Find("ProgressCoinBarMeter").GetComponent<Image>();
        equipmentProgressBar = GameObject.Find("ProgressEquipmentBarMeter").GetComponent<Image>();
        experienceProgressBar = GameObject.Find("ProgressExperienceBarMeter").GetComponent<Image>();

        //Getting text components for bars
        healthBarUpperText = GameObject.Find("HealthBarUpperText").GetComponent<Text>();
        healthBarLowerText = GameObject.Find("HealthBarLowerText").GetComponent<Text>();
        armourBarUpperText = GameObject.Find("ArmourBarUpperText").GetComponent<Text>();
        armourBarLowerText = GameObject.Find("ArmourBarLowerText").GetComponent<Text>();
        coinProgressBarText = GameObject.Find("ProgressCoinBarText").GetComponent<Text>();
        equipmentProgressBarText = GameObject.Find("ProgressEquipmentBarText").GetComponent<Text>();
        experienceProgressBarText = GameObject.Find("ProgressExperienceBarText").GetComponent<Text>();

        //Getting gameobject for spells
        spells = new GameObject[4];
        spells[0] = GameObject.Find("SpellSlot1");
        spells[1] = GameObject.Find("SpellSlot2");
        spells[2] = GameObject.Find("SpellSlot3");
        spells[3] = GameObject.Find("SpellSlot4");

        //Getting text for spells
        spellsText = new Text[4];
        spellsText[0] = spells[0].transform.GetChild(3).GetComponent<Text>();
        spellsText[1] = spells[1].transform.GetChild(3).GetComponent<Text>();
        spellsText[2] = spells[2].transform.GetChild(3).GetComponent<Text>();
        spellsText[3] = spells[3].transform.GetChild(3).GetComponent<Text>();

        //Hiding spells CD on start
        spellsText[0].text = "";
        spellsText[1].text = "";
        spellsText[2].text = "";
        spellsText[3].text = "";

        //Setting up UI
        UpdateBars();
        UpdateStats();
        levelText.text = characterExpLevel.ToString();
    }

    public void UpdateBars()
    {
        //Setting fill amounts
        coinProgressBar.fillAmount = (float)coinProgressCurrent / (float)coinProgressMax;
        equipmentProgressBar.fillAmount = (float)equipmentProgressCurrent / (float)equipmentProgressMax;
        experienceProgressBar.fillAmount = (float)experienceProgressCurrent / (float)equipmentProgressMax;
        armourBar.fillAmount = (float)armourCurrent / (float)armourMax;
        healthBar.fillAmount = (float)hpCurrent / (float)hpMax;

        //Setting text
        coinProgressBarText.text = coinProgressCurrent + "/" + coinProgressMax;
        equipmentProgressBarText.text = equipmentProgressCurrent + "/" + equipmentProgressMax;
        experienceProgressBarText.text = experienceProgressCurrent + "/" + experienceProgressMax;
        healthBarUpperText.text = hpCurrent.ToString();
        healthBarLowerText.text = hpMax.ToString();
        armourBarUpperText.text = armourCurrent.ToString();
        armourBarLowerText.text = armourMax.ToString();

        //Setting level
        levelText.text = characterExpLevel.ToString();
    }

    public void UpdateStats()
    {
        statsText.text =
              "Max health: " + hpMax.ToString() + "\n"
            + "Max armour: " + armourMax.ToString() + "\n"
            + "Base damage: " + baseDamage.ToString() + "\n"
            + "Weapon damage: " + weaponDamage.ToString() + "\n"
            + "Health regeneration: " + hpRegeneration.ToString() + "\n"
            + "Spikes: " + spikes.ToString() + "\n"
            + "Vampirism: " + vampirism.ToString() + "\n"
            + "Health by potion: " + hpByPotion.ToString() + "\n"
            + "Armour by shield: " + armourByShield.ToString() + "\n"
            + "Additional exp. gain: " + addictionalExperienceProgressByEnemy.ToString() + "\n"
            + "Additional coin gain: " + addictionalCoinProgressByCoin.ToString() + "\n"
            + "Additional eq. gain: " + addictionalEquipementProgressByShield.ToString() + "\n" +
            "Damage Reduction: " + damageReductionByArmour.ToString();
        
    }
}