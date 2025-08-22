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
    public string itemBaseStatName; // Added to support different support item effects

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
        itemBaseStatName = itemToWear.itemBaseStatName;
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
                    // Apply different support item effects based on itemBaseStatName
                    switch (itemBaseStatName)
                    {
                        case "Experience boost":
                            gl.player.addictionalExperienceProgressByEnemy += itemStat * 0.1f;
                            break;
                        case "Gold boost":
                            gl.player.addictionalCoinProgressByCoin += itemStat * 0.1f;
                            break;
                        case "Equipment boost":
                            gl.player.addictionalEquipementProgressByShield += itemStat * 0.1f;
                            break;
                        case "Spikes damage":
                            gl.player.spikes += itemStat;
                            break;
                        case "Health regen":
                            gl.player.hpRegeneration += itemStat;
                            break;
                        case "Vampirism":
                            gl.player.vampirism += itemStat;
                            break;
                    }
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
