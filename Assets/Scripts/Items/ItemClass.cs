using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Chest = 1,
    Head = 2,
    Weapon = 3,
    Support = 4
}

public class ItemClass : MonoBehaviour
{
    bool isPickable = true;

    ItemType itemType;

    //Later change this atleast support items to to be precreated by developer
    Sprite[] possibleChestArt;
    Sprite[] possibleHeadArt;
    Sprite[] possibleWeaponArt;
    Sprite[] possibleSupportArt;

    Sprite itemImage;

    int itemStat = 0;

    PlayerClass player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerClass>();
        itemImage = gameObject.GetComponent<Sprite>();
    }

    void GenerateItem()
    {
        switch (Random.Range(1, 5))
        {
            case 1:
                itemType = ItemType.Chest;
                itemImage = possibleChestArt[Random.Range(0, possibleChestArt.Length)];
                break;
            case 2:
                itemType = ItemType.Head;
                itemImage = possibleChestArt[Random.Range(0, possibleHeadArt.Length)];
                break;
            case 3:
                itemType = ItemType.Weapon;
                itemImage = possibleChestArt[Random.Range(0, possibleWeaponArt.Length)];
                break;
            case 4:
                itemType = ItemType.Support;
                itemImage = possibleChestArt[Random.Range(0, possibleSupportArt.Length)];
                break;
            default:
                break;
        }
        itemStat = player.characterEqipLevel + 1;
    }

    void FillProgressPanel()
    {

    }

    void OnMouseDown()
    {
        if (isPickable)
        {
            switch (itemType)
            {
                case ItemType.Chest:
                    //gameObject.transform.position.Set(player.chestSlot.transform.position.x, player.chestSlot.transform.position.y, player.chestSlot.transform.position.z);
                    player.chestSlot = gameObject;
                    player.armourMax += itemStat;
                    break;
                case ItemType.Head:
                    player.headSlot = gameObject;
                    player.hpByPotion += itemStat;
                    break;
                case ItemType.Weapon:
                    player.weaponSlot = gameObject;
                    player.weaponDamage += itemStat;
                    break;
                case ItemType.Support:
                    break;
                default:
                    break;
            }
            player.UpdateBars();
            player.UpdateStats();
        }
    }
}
