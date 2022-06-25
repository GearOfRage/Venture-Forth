using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameOverScreenPrefab;
    [SerializeField] ProgressLogic pl;

    GameObject gameOverScreen;
    GameObject playerObject;
    TilesGeneration tg;
    TilesField tilesField;
    TurnLogic tl;

    [HideInInspector] public GameStats gameStats;
    [HideInInspector] public PlayerClass player;

    public static Action OnGameRestart;

    Fader fader;

    void Start()
    {
        CreatePlayer();
        fader = GetComponent<Fader>();
        tg = GetComponent<TilesGeneration>();
        tilesField = GetComponent<TilesField>();
        tl = GetComponent<TurnLogic>();
        tg.FirstGenerate();
    }

    public void CheckGameOver()
    {
        if (player.hpCurrent <= 0)
        {
            player.hpCurrent = 0;
            if (pl.showedPanel != null)
            {
                Destroy(pl.showedPanel);
                pl.showedPanel = null;
            }
            fader.OpenFader();
            FindObjectOfType<AudioManager>().Play("GameOver");
            FindObjectOfType<AudioManager>().Stop("GameTheme");
            gameOverScreen = Instantiate(gameOverScreenPrefab, Vector3.zero, Quaternion.identity);
            gameOverScreen.GetComponent<Canvas>().sortingOrder = fader.screenFader.sortingOrder + 1;
        }
    }

    void CreatePlayer()
    {
        playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player = playerObject.GetComponent<PlayerClass>();
        gameStats = playerObject.GetComponent<GameStats>();
    }

    public void Restart()
    {
        Destroy(playerObject);
        CreatePlayer();
        tilesField.Clear();
        tg.FirstGenerate();
        tl.turnText.text = gameStats.turnNumber.ToString();

        FindObjectOfType<AudioManager>().Play("GameTheme");

        Destroy(gameOverScreen);
        OnGameRestart?.Invoke();
    }
}