using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Attack = 1,
    Armour = 2,
    Potion = 3,
    Coin = 4
}
public enum TileName
{
    RegularEnemy = 1,
    ExperiencePotion = 2,
    PosionPoition = 3,
    HealthPotion = 4,
    Shield = 5,
    Coin = 6,
    Sword = 7
}
public class TileClass : MonoBehaviour
{
    [SerializeField] public TileType tileType;
    [SerializeField] public TileName tileName;
}
