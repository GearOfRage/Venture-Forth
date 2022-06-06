using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    Regular = 1,
    Elite = 2,
    Boss = 3
}

public class EnemyClass : MonoBehaviour
{
    [SerializeField] public EnemyType enemyType = EnemyType.Regular;
    [SerializeField] public int attack = 1;
    [SerializeField] public int hp = 3;
    [SerializeField] public int hpMax = 3;
    [SerializeField] public int armour = 1;
    [SerializeField] public int experienceGain = 10;
    [SerializeField] public Text healthText;
    [SerializeField] public Text attackText;
    [SerializeField] public Text armourText;

    private void Awake()
    {
        healthText.text = hp.ToString();
        attackText.text = attack.ToString();
        armourText.text = armour.ToString();
    }
}
