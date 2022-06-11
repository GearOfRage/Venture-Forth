using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipItemTypeE
{
    Chest = 1,
    Head = 2,
    Weapon = 3,
    Support = 4
}

public struct EquipItemS
{
    public EquipItemTypeE itemType;
    public Sprite itemImage;
    public int itemStat;
    public string itemBaseStatName;
    public string itemMod1StatName;
    public string itemMod2StatName;
}

public class EquipItemClass : MonoBehaviour
{
    bool isOnPlayer = false;

    public EquipItemTypeE itemType;
    public Sprite itemImage;
    public int itemStat;

    GameLogic gl;
    ProgressLogic pl;

    void Start()
    {
        pl = GameObject.Find("GameManager").GetComponent<ProgressLogic>();
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }

    void OnMouseDown()
    {
        if (!isOnPlayer)
        {
            Sprite sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            switch (itemType)
            {
                case EquipItemTypeE.Chest:
                    gl.player.equipement.chestSlot = this;
                    gl.player.chestSlot.sprite = sprite;
                    gl.player.armourMax += itemStat;
                    break;
                case EquipItemTypeE.Head:
                    gl.player.equipement.headSlot = this;
                    gl.player.headSlot.sprite = sprite;
                    gl.player.hpByPotion += itemStat;
                    break;
                case EquipItemTypeE.Weapon:
                    gl.player.equipement.weaponSlot = this;
                    gl.player.weaponSlot.sprite = sprite;
                    gl.player.weaponDamage += itemStat;
                    break;
                case EquipItemTypeE.Support:
                    gl.player.equipement.itemSlot = this;
                    gl.player.itemSlot.sprite = sprite;
                    break;
                default:
                    break;
            }
            gl.player.UpdateBars();
            gl.player.UpdateStats();
            isOnPlayer = true;
        }
        pl.CloseProgressPanel();
    }
}
