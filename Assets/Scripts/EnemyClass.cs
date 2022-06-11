using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyTypeE
{
    Regular = 1,
    Elite = 2,
    Boss = 3
}

public class EnemyClass : MonoBehaviour
{
    [SerializeField] public EnemyTypeE enemyType = EnemyTypeE.Regular;
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
        UpdateStats();
    }

    public void UpdateStats()
    {
        healthText.text = hp.ToString();
        attackText.text = attack.ToString();
        armourText.text = armour.ToString();
    }
}
