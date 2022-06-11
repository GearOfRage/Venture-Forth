using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRestartButtonLogic : MonoBehaviour
{
    GameLogic gl;
    Fader fader;

    void Start()
    {
        gl = GameObject.Find("GameManager").GetComponent<GameLogic>();
        fader = GameObject.Find("GameManager").GetComponent<Fader>();
    }

    private void OnMouseDown()
    {
        gl.Restart();
        fader.CloseFader();
    }
}
