using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState { beforeStart, gamePlay, gameOver }

/// <summary>
/// Spēles režisors: punkti, stāvokļi un objektu ģenerēšana.
/// Ainā drīkst būt tikai VIENS.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;     // Singleton

    [Header("Atsauces")]
    public GameObject coinPrefab;
    public Transform coinParent;
    public TMP_Text scoreText;

    [Header("Stāvoklis")]
    public GameState gameState;
    public int score = 10;
    public int level = 0;

    void Awake()
    {
        instance = this;

        Config.coinsToGenerate = 1;   // nulējam statiskos laukus
        score = 10;
        level = 0;
        gameState = GameState.beforeStart;

        UpdateScoreText();
        Debug.Log("Gatavs? Spied F, lai sāktu!");
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.beforeStart:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    InvokeRepeating(nameof(GenerateCoins), 0f, Config.repeatRate);
                    gameState = GameState.gamePlay;
                }
                break;

            case GameState.gamePlay:
                // te var likt taimeri vai grūtības pieaugumu
                break;

            case GameState.gameOver:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
        }
    }

    void GenerateCoins()
    {
        for (int i = 0; i < Config.coinsToGenerate; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.transform.parent = coinParent;      // kārtība hierarhijā!

            float posX = Random.Range(Config.minDropX, Config.maxDropX);
            coin.transform.localPosition = new Vector3(posX, 0f, 0f);

            coin.GetComponent<Rigidbody2D>()
                .AddForce(Random.insideUnitCircle * Config.dropForce, ForceMode2D.Impulse);
        }

        level++;
        Config.coinsToGenerate++;                    // katrs vilnis - grūtāks
    }

    public void AddScore()
    {
        if (gameState != GameState.gamePlay) return;

        score++;
        UpdateScoreText();
    }

    public void RemoveScore()
    {
        if (gameState != GameState.gamePlay) return;

        score--;
        UpdateScoreText();

        if (score <= 0) GameOver();
    }

    void UpdateScoreText()
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }

    void GameOver()
    {
        CancelInvoke(nameof(GenerateCoins));
        gameState = GameState.gameOver;
        Debug.Log("Spēle beigusies! Tu izturēji līdz " + level + ". līmenim. Spied F, lai sāktu no jauna.");
    }
}
