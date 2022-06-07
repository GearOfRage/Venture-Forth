using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ExpItemType
{
    MaxHealth = 1,
    BaseDamage = 2,
}
public class ExpLvlUpLogic : MonoBehaviour
{
    PlayerClass player;

    //Later change this atleast support items to to be precreated by developer
    [SerializeField] Sprite[] statsSprites;

    [SerializeField] GameObject[] Items;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerClass>();
        FillProgressPanel();
    }

    public EquipItem GenerateItem()
    {
        EquipItem item = new();
        item.itemStat = 0;
        /*switch (Random.Range(1, 5))
        {
            case 1:
                item.itemType = EquipItemType.Chest;
                item.itemImage = possibleChestArts[Random.Range(0, possibleChestArts.Length)];
                item.itemBaseStatName = "Max armour";
                break;
            case 2:
                item.itemType = EquipItemType.Head;
                item.itemImage = possibleHeadArts[Random.Range(0, possibleHeadArts.Length)];
                item.itemBaseStatName = "HP by potion";
                break;
            case 3:
                item.itemType = EquipItemType.Weapon;
                item.itemImage = possibleWeaponArts[Random.Range(0, possibleWeaponArts.Length)];
                item.itemBaseStatName = "Weapon damage";
                break;
            case 4:
                item.itemType = EquipItemType.Support;
                item.itemImage = possibleSupportArts[Random.Range(0, possibleSupportArts.Length)];
                item.itemBaseStatName = "Gives some strange shit";
                break;
            default:
                break;
        }
        */item.itemStat = player.characterEqipLevel + 1;
        return item;
    }


    void FillProgressPanel()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            //panel.transform.Find("Item" + i.ToString()).transform.GetChild(0).gameObject
            EquipItem item = GenerateItem();
            Items[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.itemImage;
            string name = item.itemStat == 0 ? item.itemBaseStatName : item.itemBaseStatName + " +" + item.itemStat;
            Items[i].transform.GetChild(1).GetComponent<Text>().text = name;
        }


    }
}
