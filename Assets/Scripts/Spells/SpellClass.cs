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
    bool isOnPlayer = false;

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
        { SpellNameE.Conjure, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.WishForChalenge, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.WishForConsume, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.WishForTreasure, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.MagicSword, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.MagicPotion, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.TouchOfGold, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.TouchOfWar, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.Devastate, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.Thirst, "Super cool, super nice fancy-shmency spell"},
        { SpellNameE.ShieldsUp, "Super cool, super nice fancy-shmency spell"},
    };


    void Start()
    {
        pl = GameObject.Find("GameManager").GetComponent<ProgressLogic>();
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }

    void OnMouseDown()
    {
        if (isOnPlayer)
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
        for (int i = 0; i < gl.player.spells.Length; i++)
        {
            if (gl.player.spells[i] == null)
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
        gl.player.spells[emptySlotIndex] = this;
        gl.player.spellSlots[emptySlotIndex].sprite = sprite;

        isOnPlayer = true;
        pl.CloseProgressPanel();
    }
}
