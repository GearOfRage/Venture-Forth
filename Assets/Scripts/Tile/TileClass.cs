using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypeE
{
    Attack = 1,
    Armour = 2,
    Potion = 3,
    Gold = 4
}
public enum TileNameE
{
    RegularEnemy = 1,
    ExperiencePotion = 2,
    Poison = 3,
    HealthPotion = 4,
    Shield = 5,
    Coin = 6,
    Sword = 7,
    Crown = 8,
    MagicSword = 9,
    ManaPotion = 10,
    BrokenSword = 11,
    BrokenShield = 12,
    EliteEnemy = 13
}
public class TileClass : MonoBehaviour
{
    [SerializeField] public TileTypeE tileType;
    [SerializeField] public TileNameE tileName;
}
