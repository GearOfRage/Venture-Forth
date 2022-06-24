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
        
        // check which skills are learned by player and where CD can be decreased
        int availableSpellsCount = SpellClass.spellsDescription.Count;
        bool[,] hasSpellCanDecreaseCD = new bool[SpellClass.spellsDescription.Count, 2];
        int takenSlots = 0;
        int maximisedCD = 0;
        for (int j = 0; j < gl.player.spells.Length; j++)
        {
            if (gl.player.spells[j].isLearned)
            {
                takenSlots++;
                hasSpellCanDecreaseCD[(int)gl.player.spells[j].spellName, 0] = true;
                hasSpellCanDecreaseCD[(int)gl.player.spells[j].spellName, 1] = gl.player.spells[j].cooldown > 6;
                if (!hasSpellCanDecreaseCD[(int)gl.player.spells[j].spellName, 1])
                {
                    maximisedCD++;
                }
            }
        }

        if (takenSlots == gl.player.spells.Length && maximisedCD == gl.player.spells.Length)
        {
            throw new SystemException("Does not have " + number + " more spells to generate :(");
        }

        availableSpellsCount = takenSlots == gl.player.spells.Length
            ? gl.player.spells.Length - maximisedCD
            : availableSpellsCount;
        
        // create array with available spells ids
        int[] availableSpells = new int[availableSpellsCount];
        int l = 0;
        if (takenSlots == gl.player.spells.Length)
        {
            int m = 0;
            for (int j = 0; j < gl.player.spells.Length; j++)
            {
                if (hasSpellCanDecreaseCD[(int)gl.player.spells[j].spellName, 1])
                {
                    availableSpells[m] = (int)gl.player.spells[j].spellName;
                    m++;
                }
            }
        } else
        {
            for (int k = 0; k < SpellClass.spellsDescription.Count; k++)
            {
                if (!hasSpellCanDecreaseCD[k, 0] || hasSpellCanDecreaseCD[k, 1])
                {
                    availableSpells[l] = k;
                    l++;
                }
            }
        }

        // generate skills
        for (int i = 0; i < number; i++)
        {
            int randomNumber = availableSpells[UnityEngine.Random.Range(0, availableSpellsCount)];

            // todo: delete the line below
            //randomNumber = 4;

            SpellNameE spell = (SpellNameE)randomNumber;

            if (hasSpellCanDecreaseCD[randomNumber, 0])
            {
                spells[i] = new SpellS(
                    spell,
                    spellSprites[randomNumber],
                    SpellClass.spellCooldowns[spell],
                    "Decrease cooldown by 2",
                    true
                );
            }
            else
            {
                spells[i] = new SpellS(
                    spell,
                    spellSprites[randomNumber],
                    SpellClass.spellCooldowns[spell],
                    SpellClass.spellsDescription[spell],
                    false
                );
            }
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
            if (spells[i].isLeaned)
            {
                for (int j = 0; j < gl.player.spells.Length; j++)
                {
                    if (gl.player.spells[j].spellName == spells[i].spellName)
                    {
                        spellClass.myIndex = gl.player.spells[j].myIndex;
                        break;
                    }
                }
            }
        }
    }
}
