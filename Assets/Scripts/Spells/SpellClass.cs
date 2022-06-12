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

    Conjure = 0,
    WishForChalenge = 1,
    WishForConsume = 2,
    WishForTreasure = 3,
    MagicSword = 4,
    MagicPotion = 5,
    TouchOfGold = 6,
    TouchOfWar = 7,
    Devastate = 8,
    Thirst = 9,
    ShieldsUp = 10,
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
        { SpellNameE.Conjure, 30 },
        { SpellNameE.WishForChalenge, 30 },
        { SpellNameE.WishForConsume, 30 },
        { SpellNameE.WishForTreasure, 30 },
        { SpellNameE.MagicSword, 30 },
        { SpellNameE.MagicPotion, 30 },
        { SpellNameE.TouchOfGold, 30 },
        { SpellNameE.TouchOfWar, 30 },
        { SpellNameE.Devastate, 30 },
        { SpellNameE.Thirst, 30 },
        { SpellNameE.ShieldsUp, 30 },
    };

    public static Dictionary<SpellNameE, string> spellsDescription = new()
    {
        { SpellNameE.Conjure, "Super cool Conjure"},
        { SpellNameE.WishForChalenge, "Super cool WishForChalenge"},
        { SpellNameE.WishForConsume, "Super cool WishForConsume"},
        { SpellNameE.WishForTreasure, "Super cool WishForTreasure"},
        { SpellNameE.MagicSword, "Super cool MagicSword"},
        { SpellNameE.MagicPotion, "Super cool MagicPotion"},
        { SpellNameE.TouchOfGold, "Super cool TouchOfGold"},
        { SpellNameE.TouchOfWar, "Super cool TouchOfWar"},
        { SpellNameE.Devastate, "Super cool Devastate"},
        { SpellNameE.Thirst, "Super cool Thirst"},
        { SpellNameE.ShieldsUp, "Super cool ShieldsUp"},
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

        isLearned = true;
        pl.CloseProgressPanel();
    }
}
