using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLogic : MonoBehaviour
{
    public Text statsText;

    GameLogic gl;

    void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        statsText = GameObject.Find("GameOverScreenText").GetComponent<Text>();
        FillStats();
    }

    void FillStats()
    {
        statsText.text =
              "Total turns: " + gl.gameStats.turnNumber.ToString() + "\n"
              + "Total gold collected: " + gl.gameStats.collectedGold.ToString() + "\n"
              + "Total regular enemies killed: " + gl.gameStats.killedRegularEnemies.ToString() + "\n"
              + "Total elite enemies killed: " + gl.gameStats.killedEliteEnemies.ToString() + "\n"
              + "Total boss enemies killed: " + gl.gameStats.killedBossEnemies.ToString() + "\n"
              + "Total damage received: " + gl.gameStats.receivedDamage.ToString() + "\n";
    }
}
