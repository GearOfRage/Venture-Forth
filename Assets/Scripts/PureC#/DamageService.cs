using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class DamageService
{
    readonly TilesField tilesField;
    readonly GameLogic gl;

    public DamageService(TilesField tilesField, GameLogic gl)
    {
        this.tilesField = tilesField;
        this.gl = gl;
    }

    public HpArmourS CalculatePlayerHpChange()
    {
        int dmgToPlayer = 0;
        for (int i = 0; i < TilesField.gridSize; i++) //Columns
        {
            for (int j = 0; j < TilesField.gridSize; j++) //Rows
            {
                GameObject tile = tilesField.tiles[i, j];
                if (tile == null)
                {
                    continue;
                }

                EnemyClass enemy = tile.GetComponent<EnemyClass>();
                if (enemy != null && enemy.hp > 0)
                {
                    dmgToPlayer += enemy.attack;
                }
            }
        }

        return CalculateDamageWithArmour(
             dmgToPlayer,
             gl.player.armourCurrent,
             gl.player.hpCurrent,
             gl.player.damageReductionByArmour
         );
    }

    public HpArmourS CalculateDamageWithArmour(int dmg, int armour, int hp, float dmgReductionByArmour)
    {
        /**
         * Armour logic explained:
         * case 1
         * gl.player has 1 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 27
         * potentialDmgToArmour = 27 * 0.1 = 2.7
         * dmg to armour = Min(1 (currentArmour), Round(2.7), 1) = 1;
         * damagedArmour = Min(1 (currentArmour), Floor(2.7), 1) = 1
         * gl.player now has 1 - 1 armour = 0, 50 - 27 + 1 = 24
         * 
         * case 2
         * gl.player has 3 armour, 50 hp, damageReductionByArmour 0.1
         * enemy attack 17
         * potentialDmgToArmour = 17 * 0.1 = 1.7
         * dmg to armour = Min(3 (currentArmour), Round(1.7), 1) = 2;
         * damagedArmour = Min(1 (currentArmour), Floor(1.7), 1) = 1
         * gl.player now has 3 - 1 armour = 2, 50 - 17 + 2 = 35
        */
        HpArmourS res = new(hp, armour);
        if (dmg == 0)
        {
            return res;
        }

        if (armour == 0)
        {
            res.hp = hp - dmg;
            return res;
        }

        float potentialDmgToArmour = dmg * dmgReductionByArmour;
        int dmgReduction = Mathf.Max(1, Mathf.Min(armour, Mathf.RoundToInt(potentialDmgToArmour)));
        int finalDmg = dmg - dmgReduction;
        res.hp = hp - finalDmg;
        res.armour = armour - Mathf.Max(Mathf.Min(armour, Mathf.FloorToInt(potentialDmgToArmour)), 1);

        return res;
    }

}
