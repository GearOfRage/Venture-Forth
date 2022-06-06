using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnLogic : MonoBehaviour
{
    [SerializeField]
    Text turnText;

    [SerializeField]
    PlayerClass player;


    GameLogic gl;
    int turnNumber = 1;

    private void Start()
    {
        turnText.text = turnNumber.ToString();
        gl = GetComponent<GameLogic>();
    }

    public void Next()
    {
        //Some game logic happens <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        turnNumber++;
        turnText.text = turnNumber.ToString();
        //kill cherepoks first
        CalculateDamage();
    }

    public void CalculateDamage()
    {
        int dmgToPlayer = 0;
        for (int i = 0; i < GameLogic.gridSize; i++) //Columns
        {
            for (int j = 0; j < GameLogic.gridSize; j++) //Rows
            {
                GameObject tile = gl.tiles[i, j];
                if (tile == null)
                {
                    continue;
                }

                EnemyClass enemy = tile.GetComponent<EnemyClass>();
                if (enemy != null)
                {
                    dmgToPlayer += enemy.attack;
                }
            }
        }
        if (dmgToPlayer == 0)
        {
            return;
        }

        /**
         * Armour logic explained:
         * case 1
         * player has 1 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 27
         * potentialDmgToArmour = 27 * 0.1 = 2.7
         * dmg to armour = Min(1 (currentArmour), Round(2.7), 1) = 1;
         * damagedArmour = Min(1 (currentArmour), Floor(2.7), 1) = 1
         * player now has 1 - 1 armour = 0, 50 - 27 + 1 = 24
         * 
         * case 2
         * player has 3 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 17
         * potentialDmgToArmour = 17 * 0.1 = 1.7
         * dmg to armour = Min(3 (currentArmour), Round(1.7), 1) = 2;
         * damagedArmour = Min(1 (currentArmour), Floor(1.7), 1) = 1
         * player now has 3 - 1 armour = 2, 50 - 17 + 2 = 35
        */

        if (player.armourCurrent == 0)
        {
            player.hpCurrent -= dmgToPlayer;
        }
        else
        {
            float potentialDmgToArmour = dmgToPlayer * player.damageReductionByArmour;
            int dmgReduction = Mathf.Max(1, Mathf.Min(player.armourCurrent, Mathf.RoundToInt(potentialDmgToArmour)));
            int finalDmgToPlayer = dmgToPlayer - dmgReduction;
            player.armourCurrent -= Mathf.Max(Mathf.Min(player.armourCurrent, Mathf.FloorToInt(potentialDmgToArmour)), 1);
            player.hpCurrent -= finalDmgToPlayer;
        }

        if (player.hpCurrent > 0)
        {
            player.UpdateBars();
        }
        else
        {
            player.hpCurrent = 0;
            player.UpdateBars();
            gl.GameOver();
        }
    }
}
