using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Attack = 1,
    Armour = 2,
    Potion = 3,
    Coin = 3
}
public class TileClass : MonoBehaviour
{
    [SerializeField] public TileType tileType;
    [SerializeField] public string tileName;
}
