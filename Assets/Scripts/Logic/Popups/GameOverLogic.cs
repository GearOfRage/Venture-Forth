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
              "Total turns: " + gl.gameStats.turnNumber.ToString();
    }

}
