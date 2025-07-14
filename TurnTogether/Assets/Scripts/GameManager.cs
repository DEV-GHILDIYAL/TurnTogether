using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // =================== GAME STATE ===================
    public bool hasGameStarted = false;
    public bool isGameOver = false;

    // =================== UI REFERENCES ===================
    public GameObject gameOverPanel;
    public GameObject MainMenuUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI coinCollectedText;
    public TextMeshProUGUI starCollectedText;

    // =================== AUDIO ===================
    public AudioClip audioClip;
    public AudioSource audioSource;
    public AudioClip buttonClick;

    // =================== GAME DATA ===================
    public float playerSpeed = 5f;
    private int score = 0;
    private int highScore = 0;
    private int coinCount = 0;
    private int starCount = 0;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
    public void ButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }
    void Start()
    {
        MainMenuUI.SetActive(true);
        score = 0;
        coinCount = 0;
        starCount = 0;
        scoreText.text = score.ToString();
    }

    public void StartGame()
    {
        if (hasGameStarted) return;

        hasGameStarted = true;
        isGameOver = false;
        score = 0;
        UpdateScoreUI();
        Debug.Log("🎮 Game Started!");
        MainMenuUI.SetActive(false);
    }

    void Update()
    {
        // Real-time update of speed display
        if (speedText != null)
            speedText.text = "Speed: " + playerSpeed.ToString("F1");

        // Optional: Show high score always
        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }
    public void AddCoin(int gained)
    {
        coinCount += gained;
    }

    public void AddStar()
    {
        starCount++;
    }


    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        // Show final score
        if (finalScoreText != null)
            finalScoreText.text = "Your Score: " + score;

        if (coinCollectedText != null)
            coinCollectedText.text = "Coins Collected: " + coinCount;

        if (starCollectedText != null)
            starCollectedText.text = "Stars Collected: " + starCount;


        // High score check
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Show high score if field is assigned
        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;

        Debug.Log("💀 Game Over! Score: " + score + " | HighScore: " + highScore);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HackStar()
    {
        int currentStars = PlayerPrefs.GetInt("Stars", 0);
        PlayerPrefs.SetInt("Stars", currentStars + 100);
            PlayerPrefs.Save();
    }
}
