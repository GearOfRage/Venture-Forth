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
    [SerializeField] public int attack;
    [SerializeField] public int health;
    [SerializeField] public int armour;
    [SerializeField] public int experienceGain;
    [SerializeField] public Text healthText;
    [SerializeField] public Text attackText;
    [SerializeField] public Text armourText;

    private void Awake()
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        armourText.text = armour.ToString();
    }

}
