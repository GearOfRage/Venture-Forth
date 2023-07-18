using System;
using UnityEngine;
using VContainer;

public class GameLogic : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameOverScreenPrefab;
    [SerializeField] ProgressLogic pl;

    GameObject gameOverScreen;
    GameObject playerObject;
    TurnLogic tl;

    [Inject]
    readonly TilesGeneration tg;

    [Inject]
    readonly TilesField tilesField;

    [Inject]
    public GameStats gameStats;

    [HideInInspector] public PlayerClass player;

    public static Action OnGameRestart;

    Fader fader;

    void Start()
    {
        CreatePlayer();
        fader = GetComponent<Fader>();
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
    }

    public void Restart()
    {
        Destroy(playerObject);
        CreatePlayer();
        tilesField.Clear();
        tg.FirstGenerate();
        gameStats.turnNumber = 1;
        
        tl.turnText.text = gameStats.turnNumber.ToString();

        
        
        FindObjectOfType<AudioManager>().Play("GameTheme");

        Destroy(gameOverScreen);
        OnGameRestart?.Invoke();
    }
}