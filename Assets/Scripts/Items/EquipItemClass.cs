using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipItemType
{
    Chest = 1,
    Head = 2,
    Weapon = 3,
    Support = 4
}

public struct EquipItem
{
    public EquipItemType itemType;
    public Sprite itemImage;
    public int itemStat;
    public string itemBaseStatName;
    public string itemMod1StatName;
    public string itemMod2StatName;
}

public class EquipItemClass : MonoBehaviour
{
    bool isOnPlayer = false;

    public EquipItemType itemType;
    public Sprite itemImage;
    public int itemStat;

    PlayerClass player;
    ProgressLogic pl;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerClass>();
        pl = GameObject.Find("GameManager").GetComponent<ProgressLogic>();
    }

    void OnMouseDown()
    {
        if (!isOnPlayer)
        {
            Sprite sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            switch (itemType)
            {
                case EquipItemType.Chest:
                    player.equipement.chestSlot = this;
                    player.chestSlot.GetComponent<SpriteRenderer>().sprite = sprite;
                    player.armourMax += itemStat;
                    break;
                case EquipItemType.Head:
                    player.equipement.headSlot = this;
                    player.headSlot.GetComponent<SpriteRenderer>().sprite = sprite;
                    player.hpByPotion += itemStat;
                    break;
                case EquipItemType.Weapon:
                    player.equipement.weaponSlot = this;
                    player.weaponSlot.GetComponent<SpriteRenderer>().sprite = sprite;
                    player.weaponDamage += itemStat;
                    break;
                case EquipItemType.Support:
                    player.equipement.itemSlot = this;
                    player.itemSlot.GetComponent<SpriteRenderer>().sprite = sprite;
                    break;
                default:
                    break;
            }
            player.UpdateBars();
            player.UpdateStats();
            isOnPlayer = true;
        }
        pl.CloseProgressPanel();
    }
}
