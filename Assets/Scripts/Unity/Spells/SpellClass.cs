using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellNameE
{
    // ������� ��������� - ���� ��������� ���� ���������� ������� �� ��������.
    // ������� ��������� - ���������� ��������� ���� � �������� �����.
    // �������� - ���������� ��� ����� � ����� �����. (���� �� ����� ����� ����� �� �������� �����)
    // ���������� ��� - ���������� ��������� ��� � ���������� ���. (��� ���� ����� 5� ����� ������)
    // ������������ - ��� ����� ����� ����� ������� ������������� ����� ��������.
    // ������� ������ - ���������� ��� ���� � ������.
    // ������� ����� - ���������� ��� ������ � ����.
    // ��������� ����� - ����� � ���� ���� ���������� � ��� ���� ������ ��.
    // ������ - ������� ����� ���� ������ �� 0.
    // ����� - �������� ��� �����.
    // ��������� ������ - �������� ��� ����.
    // ����������� - �������� ��� ����� �� ���������. (����� ������ � ������)

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
    public bool isLeaned;

    public SpellS(SpellNameE n, Sprite img, int cd, string text, bool l)
    {
        spellImage = img;
        spellName = n;
        cooldown = cd;
        description = text;
        isLeaned = l;
    }
}

public class SpellClass : MonoBehaviour
{
    public bool isLearned = false; //does player have this spell in the spellSlot

    public int myIndex = -1;
    public SpellNameE spellName;
    public Sprite spellImage;
    public string description;
    public int cooldown;
    public int currentCooldown;

    GameLogic gl;
    ProgressLogic pl;

    public static Dictionary<SpellNameE, int> spellCooldowns = new()
    {
        { SpellNameE.Conversion, 5 },
        { SpellNameE.WishForChalenge, 7 },
        { SpellNameE.WishForConsume, 9 },
        { SpellNameE.WishForTreasure, 4 },
        { SpellNameE.MagicSword, 8 },
        { SpellNameE.MagicPotion, 6 },
        { SpellNameE.TouchOfGold, 3 },
        { SpellNameE.TouchOfWar, 4 },
        { SpellNameE.Break, 6 },
        { SpellNameE.Thirst, 10 },
        { SpellNameE.ShieldsUp, 4 },
        { SpellNameE.Knowledge, 10 },
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

    public static Action<SpellClass> OnCast;

    public void Learn(SpellClass spellToLearn)
    {
        if (isLearned)
        {
            cooldown -= 2;
        }
        else
        {
            isLearned = true;
            spellName = spellToLearn.spellName;
            spellImage = spellToLearn.spellImage;
            description = spellToLearn.description;
            cooldown = spellToLearn.cooldown;
            currentCooldown = spellToLearn.currentCooldown;
            myIndex = spellToLearn.myIndex;
        }
    }

    void Start()
    {
        pl = GameObject.Find("GameManager").GetComponent<ProgressLogic>();
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        TurnLogic.OnTurnEnd += TurnEndHandler;
        GameLogic.OnGameRestart += RestartHandler;
    }

    private void RestartHandler()
    {
        isLearned = false;
        currentCooldown = 0;
        spellImage = null;
        if (myIndex >= 0 && gl.player.spellSlots.Length > myIndex)
        {
            gl.player.spellSlots[myIndex].color = new Color(1f, 1f, 1f, 1f);
        }
        myIndex = -1;
        PlayerClass.onStatUpdate?.Invoke();
    }

    private void OnDestroy()
    {
        TurnLogic.OnTurnEnd -= TurnEndHandler;
        GameLogic.OnGameRestart -= RestartHandler;
    }

    void TurnEndHandler()
    {
        if (currentCooldown > 0)
        {
            currentCooldown--;
            if (currentCooldown == 0)
            {
                gl.player.spellSlots[myIndex].color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    void OnMouseDown()
    {
        if (isLearned)
        {
            if (currentCooldown == 0)
            {
                OnCast?.Invoke(this);
                gl.player.spellSlots[myIndex].color = new Color(0.5f, 0.5f, 0.5f, 1f);

                currentCooldown = cooldown;
                PlayerClass.onStatUpdate.Invoke();
            }
        }
        else
        {
            if (spellImage != null) ChooseSpellFromLvlUpPanel();
        }
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
        throw new Exception("No empty slot for the spell ><'");
    }

    void ChooseSpellFromLvlUpPanel()
    {
        Sprite sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;

        myIndex = myIndex == -1 ? FindEmptySlotIndex() : myIndex;
        gl.player.spells[myIndex].Learn(this);
        gl.player.spellSlots[myIndex].sprite = sprite;
        isLearned = true;
        pl.CloseProgressPanel();
    }
}
