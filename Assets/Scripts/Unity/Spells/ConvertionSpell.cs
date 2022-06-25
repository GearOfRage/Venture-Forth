using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertionSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.Conversion;

    TilesGeneration tg;
    void Start()
    {
        tg = GameObject.Find("GameManager").GetComponent<TilesGeneration>();
        //add subscription to spellclass cast event
        SpellClass.OnCast += Cast;
    }

    void Cast(SpellClass castedSpell)
    {
        if (castedSpell.spellName != mySpellName)
        {
            return;
        }
        FindObjectOfType<AudioManager>().Play("ConversionSpellCast");
        tg.tilesField.Clear();
        tg.FirstGenerate();
    }
}
