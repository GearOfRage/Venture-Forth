using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipLvlUpLogic : MonoBehaviour
{
    GameLogic gl;

    //Later change this atleast support items to to be precreated by developer
    [SerializeField] Sprite[] possibleChestArts;
    [SerializeField] Sprite[] possibleHeadArts;
    [SerializeField] Sprite[] possibleWeaponArts;
    [SerializeField] Sprite[] possibleSupportArts;

    [SerializeField] GameObject[] Items;

    void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        gl.player.equipmentProgressMax += 5;
        gl.player.UpdateBars();
        PlayerClass.onStatUpdate.Invoke();
        FillProgressPanel();
    }

    public EquipItemS GenerateItem()
    {
        EquipItemS item = new();
        item.itemStat = 0;
        switch (Random.Range(1, 5))
        {
            case 1:
                item.itemType = EquipItemTypeE.Chest;
                item.itemImage = possibleChestArts[Random.Range(0, possibleChestArts.Length)];
                item.itemBaseStatName = "Max armour";
                break;
            case 2:
                item.itemType = EquipItemTypeE.Head;
                item.itemImage = possibleHeadArts[Random.Range(0, possibleHeadArts.Length)];
                item.itemBaseStatName = "HP by potion";
                break;
            case 3:
                item.itemType = EquipItemTypeE.Weapon;
                item.itemImage = possibleWeaponArts[Random.Range(0, possibleWeaponArts.Length)];
                item.itemBaseStatName = "Weapon damage";
                break;
            case 4:
                item.itemType = EquipItemTypeE.Support;
                item.itemImage = possibleSupportArts[Random.Range(0, possibleSupportArts.Length)];
                item.itemBaseStatName = "Gives some strange shit";
                break;
            default:
                break;
        }
        item.itemStat = gl.player.characterEqipLevel + 1;
        return item;
    }


    void FillProgressPanel()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            //panel.transform.Find("Item" + i.ToString()).transform.GetChild(0).gameObject
            EquipItemS item = GenerateItem();
            Items[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.itemImage;
            string name = item.itemStat == 0 ? item.itemBaseStatName : item.itemBaseStatName + " +" + item.itemStat;
            Items[i].transform.GetChild(1).GetComponent<Text>().text = name;
        }


    }
}
