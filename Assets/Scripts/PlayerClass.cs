using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterClassE
{
    Brawler = 1,
    TreasureHunter = 2,
    Sorcerer = 3
}

public class PlayerClass : MonoBehaviour
{
    public CharacterClassE characterClass;

    [Header("Internal data")]
    public SpellClass[] spells;
    public EquipItemClass headItem;
    public EquipItemClass weaponItem;
    public EquipItemClass itemItem;
    public EquipItemClass chestItem;

    [Header("Stats")]

    //Stats
    public int characterEqipLevel = 0;
    public int characterGoldLevel = 0;
    public int characterExpLevel = 1;
    public int hpMax = 40; //Basic stat
    public int hpCurrent = 40;
    public int armourMax = 5; //Basic stat
    public int armourCurrent = 5;
    public int baseDamage = 2; //Basic stat
    public int weaponDamage = 2;
    public int coinProgressMax = 10;
    public int equipmentProgressMax = 10;
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
    public SpriteRenderer chestSlot;
    public SpriteRenderer headSlot;
    public SpriteRenderer weaponSlot;
    public SpriteRenderer itemSlot;

    //Spells gameobjects
    public SpriteRenderer[] spellSlots;

    //Spells text
    public Text[] spellsText;

    public static Action onStatUpdate;
    private void Init()
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
        coinProgressBar.fillAmount = (float)coinProgressCurrent / (float)coinProgressMax;
        equipmentProgressBar.fillAmount = (float)equipmentProgressCurrent / (float)equipmentProgressMax;
        experienceProgressBar.fillAmount = (float)experienceProgressCurrent / (float)experienceProgressMax;
        armourBar.fillAmount = (float)armourCurrent / (float)armourMax;
        healthBar.fillAmount = (float)hpCurrent / (float)hpMax;

        //Getting text components for bars
        healthBarUpperText = GameObject.Find("HealthBarUpperText").GetComponent<Text>();
        healthBarLowerText = GameObject.Find("HealthBarLowerText").GetComponent<Text>();
        armourBarUpperText = GameObject.Find("ArmourBarUpperText").GetComponent<Text>();
        armourBarLowerText = GameObject.Find("ArmourBarLowerText").GetComponent<Text>();
        coinProgressBarText = GameObject.Find("ProgressCoinBarText").GetComponent<Text>();
        equipmentProgressBarText = GameObject.Find("ProgressEquipmentBarText").GetComponent<Text>();
        experienceProgressBarText = GameObject.Find("ProgressExperienceBarText").GetComponent<Text>();

        //Initing equip
        GameObject chest = GameObject.Find("ChestSlot");
        chestItem = chest.GetComponent<EquipItemClass>();
        chestSlot = chest.transform.GetChild(1).GetComponent<SpriteRenderer>();

        GameObject head = GameObject.Find("HeadSlot");
        headItem = head.GetComponent<EquipItemClass>();
        headSlot = head.transform.GetChild(1).GetComponent<SpriteRenderer>();

        GameObject weapon = GameObject.Find("WeaponSlot");
        weaponItem = weapon.GetComponent<EquipItemClass>();
        weaponSlot = weapon.transform.GetChild(1).GetComponent<SpriteRenderer>();

        GameObject item = GameObject.Find("ItemSlot");
        itemItem = item.GetComponent<EquipItemClass>();
        itemSlot = item.transform.GetChild(1).GetComponent<SpriteRenderer>();

        chestSlot.sprite = null;
        headSlot.sprite = null;
        weaponSlot.sprite = null;
        itemSlot.sprite = null;

        //Initing spells
        spells = new SpellClass[4];
        spellSlots = new SpriteRenderer[4];
        spellsText = new Text[4];
        for (int i = 0; i < spells.Length; i++)
        {
            GameObject spell = GameObject.Find("SpellSlot" + (i + 1).ToString());
            //Saving SpellClass
            spells[i] = spell.GetComponent<SpellClass>();
            //Getting gameobject for spells
            spellSlots[i] = spell.transform.GetChild(1).GetComponent<SpriteRenderer>();
            spellSlots[i].sprite = null;
            //Getting text for spells
            spellsText[i] = spell.transform.GetChild(3).GetComponent<Text>();
            //Hiding spells CD on start
            spellsText[i].text = "";
        }

        //Setting up UI
        UpdateBars();
        UpdateStats();
        levelText.text = characterExpLevel.ToString();
    }

    private void Start()
    {
        Init();
        onStatUpdate += UpdateStats;
        TurnLogic.OnTurnEnd += UpdateStats;
        TurnLogic.OnTurnEnd += UpdateBars;
    }
    public void UpdateBars()
    {
        //Setting fill amounts
        StartCoroutine(SmoothBarFill(coinProgressBar, coinProgressCurrent, coinProgressMax));
        StartCoroutine(SmoothBarFill(equipmentProgressBar, equipmentProgressCurrent, equipmentProgressMax));
        StartCoroutine(SmoothBarFill(experienceProgressBar, experienceProgressCurrent, experienceProgressMax));
        StartCoroutine(SmoothBarFill(armourBar, armourCurrent, armourMax));
        StartCoroutine(SmoothBarFill(healthBar, hpCurrent, hpMax));

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

    IEnumerator SmoothBarFill(Image image, int current, int max)
    {
        float fillSmoothness = 0.005f;
        float prevFill = image.fillAmount;
        float currFill = (float)current / max;

        while (currFill != prevFill)
        {
            if (currFill > prevFill)
                prevFill = Mathf.Min(prevFill + fillSmoothness, currFill);
            if (currFill < prevFill)
                prevFill = Mathf.Max(prevFill - fillSmoothness, currFill);

            image.fillAmount = prevFill;
            yield return null;
        }
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

        for (int i = 0; i < spellsText.Length; i++)
        {
            spellsText[i].text = spells[i].currentCooldown == 0 ? "" : spells[i].currentCooldown.ToString();
        }

    }
}