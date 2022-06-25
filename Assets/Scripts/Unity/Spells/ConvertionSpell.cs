using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ConvertionSpell : MonoBehaviour
{
    readonly SpellNameE mySpellName = SpellNameE.Conversion;

    [Inject]
    TilesGeneration tg;
    void Start()
    {
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
