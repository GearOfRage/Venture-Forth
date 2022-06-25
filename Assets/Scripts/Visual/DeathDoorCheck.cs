using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDoorCheck : MonoBehaviour
{

    bool isDeathOnDoor = false;
    SpriteRenderer deathDoorSpriteRenderer;
    TurnLogic tl;

    void Start()
    {
        TurnLogic.OnTurnEnd += CheckDeathDoor;
        GameLogic.OnGameRestart += CheckDeathDoor;
        TryGetComponent(out deathDoorSpriteRenderer);
        tl = FindObjectOfType<TurnLogic>();
    }


    void CheckDeathDoor()
    {
        HpArmourS hpArmour = tl.CalculatePlayerHpChange();

        if (hpArmour.hp > 0 && isDeathOnDoor)
        {
            //stop 
            isDeathOnDoor = false;
            deathDoorSpriteRenderer.color = new(.75f, 0f, 0f, 0f);
        }
        else if (hpArmour.hp <= 0 && !isDeathOnDoor)
        {
            //start
            isDeathOnDoor = true;
            StartCoroutine(StartFlashing());
        }
    }

    IEnumerator StartFlashing()
    {
        float smoothness = 2f;
        Color color = new(.75f, 0f, 0f, 0f);
        int direction = 1;
        while (isDeathOnDoor)
        {
            if (deathDoorSpriteRenderer.color.a > .5f)
            {
                direction = -1;
            }
            else if (deathDoorSpriteRenderer.color.a < 0f)
            {
                direction = 1;
            }

            color.a += Time.deltaTime / smoothness * direction;

            deathDoorSpriteRenderer.color = color;
            yield return null;
        }
    }
}
