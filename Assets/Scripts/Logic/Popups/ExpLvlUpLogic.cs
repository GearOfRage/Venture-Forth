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
        gl.player.experienceProgressMax += 20;
        gl.player.UpdateBars();
        PlayerClass.onStatUpdate.Invoke();
        FillProgressPanel();
    }

    public SpellS[] GenerateSpells(int number)
    {
        SpellS[] spells = new SpellS[number];
        int[] randomNumbers = new int[number];
        for (int i = 0; i < number; i++)
        {
            int randomNumber;
            while (true)
            {
                randomNumber = UnityEngine.Random.Range(0, SpellClass.spellsDescription.Count);
                bool repeat = false;
                for (int j = 0; j < number; j++)
                {
                    if (randomNumbers[j] == randomNumber)
                    {
                        repeat = true;
                    }
                }
                if (!repeat)
                {
                    break;
                }
            }
            // todo: delete the line below
            randomNumber = 7;
            randomNumbers[i] = randomNumber;

            SpellNameE spell = (SpellNameE)randomNumber;
            spells[i] = new SpellS(
                spell,
                spellSprites[randomNumber],
                SpellClass.spellCooldowns[spell],
                SpellClass.spellsDescription[spell]
            );
        }


        return spells;
    }

    void FillProgressPanel()
    {
        SpellS[] spells = GenerateSpells(4);
        // TODO: for now fill only with spells, add stats in the future
        for (int i = 0; i < spells.Length; i++)
        {
            Items[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spells[i].spellImage;
            Items[i].transform.GetChild(1).GetComponent<Text>().text = spells[i].description;
            SpellClass spellClass = Items[i].GetComponent<SpellClass>();
            spellClass.description = spells[i].description;
            spellClass.spellName = spells[i].spellName;
            spellClass.cooldown = spells[i].cooldown;
            spellClass.currentCooldown = 0;
            spellClass.spellImage = spells[i].spellImage;
        }
    }
}
