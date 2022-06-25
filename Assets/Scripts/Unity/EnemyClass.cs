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

    [SerializeField] public GameObject killMark;

    GameLogic gl;

    private void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();

        ScaleStats();
        UpdateStats();
    }

    void ScaleStats()
    {
        attack = gl.player.characterEqipLevel + 1 + (gl.gameStats.turnNumber / 10);
        hp = gl.player.characterEqipLevel + gl.player.characterExpLevel + gl.player.characterGoldLevel + (gl.gameStats.turnNumber / 8) + 2;
        hpMax = gl.player.characterEqipLevel + gl.player.characterExpLevel + gl.player.characterGoldLevel + (gl.gameStats.turnNumber / 8);
        armour = gl.player.characterEqipLevel + 1;
    }

    public void UpdateStats()
    {
        healthText.text = hp.ToString();
        attackText.text = attack.ToString();
        armourText.text = armour.ToString();
    }
}
