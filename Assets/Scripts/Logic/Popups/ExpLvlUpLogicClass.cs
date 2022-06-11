using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ExpItemTypeE
{
    MaxHealth = 1,
    BaseDamage = 2,
}
public class ExpLvlUpLogic : MonoBehaviour
{
    GameLogic gl;

    //Later change this atleast support items to to be precreated by developer
    [SerializeField] Sprite[] statsSprites;
    [SerializeField] Sprite[] spellSprites;

    [SerializeField] GameObject[] Items;

    void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        FillProgressPanel();
    }

    public SpellS GenerateSpell()
    {
        int randomNumber = UnityEngine.Random.Range(0, SpellClass.spellCooldowns.Count);
        SpellNameE spell = (SpellNameE)randomNumber;
        return new SpellS(
            spell,
            spellSprites[randomNumber],
            SpellClass.spellCooldowns[spell],
            SpellClass.spellsDescription[spell]
            );
    }

    void FillProgressPanel()
    {
        // TODO: for now fill only with spells, add stats in the future
        for (int i = 0; i < Items.Length; i++)
        {
            SpellS spell = GenerateSpell();
            Items[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spell.spellImage;
            Items[i].transform.GetChild(1).GetComponent<Text>().text = spell.description;
            SpellClass spellClass = Items[i].GetComponent<SpellClass>();
            spellClass.description = spell.description;
            spellClass.spellName = spell.spellName;
            spellClass.cooldown = spell.cooldown;
            spellClass.currentCooldown = 0;
            spellClass.spellImage = spell.spellImage;
        }
    }
}
