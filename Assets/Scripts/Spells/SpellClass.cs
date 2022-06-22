using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellNameE
{
    // Желание пожирания - весь нанесённый урон воссановит столько же здоровья.
    // Желание испытание - превращает случайный тайл в элитного врага.
    // Познание - превращает все зелья в зелья опыта. (опыт за зелья равно опыту за обычного врага)
    // Магический меч - превращает случайный меч в магический меч. (его урон равен 5х урона оружия)
    // Сокровищница - все новые тайлы после цепочки гарантировано будут монетами.
    // Касание Мидаса - превращает все мечи в монеты.
    // Касание Войны - превращает все монеты в мечи.
    // Волшебное зелье - зелья в этом ходу восполняют в два раза больше ХП.
    // Разлом - снижает броню всех врагов до 0.
    // Жажда - собирает все зелья.
    // Закрепить защиту - собирает все щиты.
    // Превращение - заменяет все тайлы на случайные. (кроме элиток и боссов)

    Conversion = 0,
    WishForChalenge = 1,
    WishForConsume = 2,
    WishForTreasure = 3,
    MagicSword = 4,
    MagicPotion = 5,
    TouchOfGold = 6,
    TouchOfWar = 7,
    Break = 8,
    Thirst = 9,
    ShieldsUp = 10,
    Knowledge = 11
}

public struct SpellS
{
    public SpellNameE spellName;
    public Sprite spellImage;
    public int cooldown;
    public string description;

    public SpellS(SpellNameE n, Sprite img, int cd, string text)
    {
        spellImage = img;
        spellName = n;
        cooldown = cd;
        description = text;
    }
}

public class SpellClass : MonoBehaviour
{
    bool isLearned = false; //does player have this spell in the spellSlot

    public SpellNameE spellName;
    public Sprite spellImage;
    public string description;
    public int cooldown;
    public int currentCooldown;

    GameLogic gl;
    ProgressLogic pl;

    public static Dictionary<SpellNameE, int> spellCooldowns = new()
    {
        { SpellNameE.Conversion, 30 },
        { SpellNameE.WishForChalenge, 30 },
        { SpellNameE.WishForConsume, 30 },
        { SpellNameE.WishForTreasure, 30 },
        { SpellNameE.MagicSword, 30 },
        { SpellNameE.MagicPotion, 30 },
        { SpellNameE.TouchOfGold, 30 },
        { SpellNameE.TouchOfWar, 30 },
        { SpellNameE.Break, 30 },
        { SpellNameE.Thirst, 30 },
        { SpellNameE.ShieldsUp, 30 },
        { SpellNameE.Knowledge, 30 },
    }; 

    //public static Dictionary<SpellNameE, int> spellSprite = new()
    //{
    //    { SpellNameE.Conversion, 30 },
    //    { SpellNameE.WishForChalenge, 30 },
    //    { SpellNameE.WishForConsume, 30 },
    //    { SpellNameE.WishForTreasure, 30 },
    //    { SpellNameE.MagicSword, 30 },
    //    { SpellNameE.MagicPotion, 30 },
    //    { SpellNameE.TouchOfGold, 30 },
    //    { SpellNameE.TouchOfWar, 30 },
    //    { SpellNameE.Break, 30 },
    //    { SpellNameE.Thirst, 30 },
    //    { SpellNameE.ShieldsUp, 30 },
    //    { SpellNameE.Knowledge, 30 },
    //};

    public static Dictionary<SpellNameE, string> spellsDescription = new()
    {
        { SpellNameE.Conversion, "Conjure\nChanges all tiles. Exepts for Bosses and Elites."},
        { SpellNameE.WishForChalenge, "Wish For Chalenge\nTransforms random tile in Elite."},
        { SpellNameE.WishForConsume, "Wish For Consume\nAll the damage inflicted to enemies this turn, will restore the same amount of health."},
        { SpellNameE.WishForTreasure, "Wish For Treasure\nAll new tiles after this turn will be coins."},
        { SpellNameE.MagicSword, "Magic Sword\nTransforms random sword in magic sword. Magic sword have 3x amount of weapon damage."},
        { SpellNameE.MagicPotion, "Magic Potion\nTransforms random potion in magic potion. Magic potion have 3x amount of health by potion."},
        { SpellNameE.TouchOfGold, "Touch Of Gold\nTransforms all swords in coins."},
        { SpellNameE.TouchOfWar, "Touch Of War\nTransforms all coins in swords."},
        { SpellNameE.Break, "Break\nSets armour of all enemies to 0."},
        { SpellNameE.Thirst, "Thirst\nCollects all potions."},
        { SpellNameE.ShieldsUp, "Shields Up\nCollects all shields."},
        { SpellNameE.Knowledge, "Knowledge\nTransforms all potions in experience potions. Experience potion gives experience equals amount of health by potion."},
    };

    public void Learn(SpellClass spellToLearn)
    {
        isLearned = true;
        spellName = spellToLearn.spellName;
        spellImage = spellToLearn.spellImage;
        description = spellToLearn.description;
        cooldown = spellToLearn.cooldown;
        currentCooldown = spellToLearn.currentCooldown;
    }

    void Start()
    {
        pl = GameObject.Find("GameManager").GetComponent<ProgressLogic>();
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }
    
    void OnMouseDown()
    {
        if (isLearned)
        {
            CastSpell();
        }
        else
        {
            ChooseSpellFromLvlUpPanel();
        }
    }

    void CastSpell()
    {

    }

    int FindEmptySlotIndex()
    {
        for (int i = 0; i < gl.player.spellSlots.Length; i++)
        {
            if (gl.player.spellSlots[i].sprite == null)
            {
                return i;
            }
        }
        throw new System.Exception("No empty slot for the spell ><'");
    }

    void ChooseSpellFromLvlUpPanel()
    {
        Sprite sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;

        int emptySlotIndex = FindEmptySlotIndex();
        gl.player.spells[emptySlotIndex].Learn(this);
        gl.player.spellSlots[emptySlotIndex].sprite = sprite;

        Debug.Log(gl.player.spells[emptySlotIndex].spellName);
        Debug.Log(gl.player.spellSlots[emptySlotIndex].sprite);
        isLearned = true;
        pl.CloseProgressPanel();
    }
}
