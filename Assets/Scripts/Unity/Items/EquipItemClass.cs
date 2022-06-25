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

    public void Wear(EquipItemClass itemToWear)
    {
        isOnPlayer = true;
        itemType = itemToWear.itemType;
        itemImage = itemToWear.itemImage;
        itemStat = itemToWear.itemStat;
    }

    void OnMouseDown()
    {
        if (!isOnPlayer)
        {
            Sprite sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            switch (itemType)
            {
                case EquipItemTypeE.Chest:
                    gl.player.chestItem.Wear(this);
                    gl.player.chestSlot.sprite = sprite;
                    gl.player.armourMax += itemStat;
                    break;
                case EquipItemTypeE.Head:
                    gl.player.headItem.Wear(this);
                    gl.player.headSlot.sprite = sprite;
                    gl.player.hpByPotion += itemStat;
                    break;
                case EquipItemTypeE.Weapon:
                    gl.player.weaponItem.Wear(this);
                    gl.player.weaponSlot.sprite = sprite;
                    gl.player.weaponDamage += itemStat;
                    break;
                case EquipItemTypeE.Support:
                    gl.player.itemItem.Wear(this);
                    gl.player.itemSlot.sprite = sprite;
                    break;
                default:
                    break;
            }
            gl.player.UpdateBars();
            PlayerClass.onStatUpdate.Invoke();
            isOnPlayer = true;
        }
        pl.CloseProgressPanel();
    }
}
