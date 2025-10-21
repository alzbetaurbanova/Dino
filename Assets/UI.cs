#pragma warning disable 0618
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;
    public static bool isGameOver = false;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI yourHighscore;
    [SerializeField] private TextMeshProUGUI yourScoreText;
    [SerializeField] private TextMeshProUGUI meteorShowerText;
    [SerializeField] private float meteorTextDuration = 3f;

    private int scoreValue = 0;
    private int currentHighscore = 0;

    [SerializeField] private GameObject tryAgainButton;
    [SerializeField] private GameObject QuitButton;

    [Space]
    [SerializeField] private GunController GunController;
    [SerializeField] private GameObject gameOverScreen;
    private float gameTime = 0f;

    [SerializeField] private CameraScroll cameraScroll;
    [SerializeField] private UnityEngine.UI.Toggle shakeToggle;
    public bool isCameraShakeEnabled = true;

    [SerializeField] private UnityEngine.UI.Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        gameTime = 0f;
        isGameOver = false;
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);

        currentHighscore = PlayerPrefs.GetInt("Highscore", 0);

        if (cameraScroll != null)
        {
            cameraScroll.enableShake = PlayerPrefs.GetInt("CameraShake", 1) == 1;
        }

        if (shakeToggle != null)
        {
            shakeToggle.onValueChanged.RemoveAllListeners();
            shakeToggle.isOn = cameraScroll.enableShake;
            shakeToggle.onValueChanged.AddListener(ToggleCameraShake);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            gameTime += Time.deltaTime;
            timerText.text = gameTime.ToString("#,#");
        }
    }

    private string GetLevelKey(string suffix)
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return sceneName + "_" + suffix;
    }


    public void AddScore()
    {
        scoreValue++;
        scoreText.text = scoreValue.ToString("#,#");
        IncrementTotalMeteors(1);
    }

    public void UpdateAmmoInfo(int currentBullets, int maxBullets)
    {
        ammoText.text = currentBullets + "/" + maxBullets;
    }
    private void IncrementTotalMeteors(int amount)
    {
        string totalMeteorsKey = GetLevelKey("TotalMeteors");
        int totalMeteors = PlayerPrefs.GetInt(totalMeteorsKey, 0);
        totalMeteors += amount;
        PlayerPrefs.SetInt(totalMeteorsKey, totalMeteors);
        PlayerPrefs.Save();
    }

    public void CheckForHighscore(int scoreValue)
    {

        string levelHighscoreKey = GetLevelKey("Highscore");
        int bestScore = PlayerPrefs.GetInt(levelHighscoreKey, 0);

        if (scoreValue > bestScore)
        {
            PlayerPrefs.SetInt(levelHighscoreKey, scoreValue);
            PlayerPrefs.Save(); 
            currentHighscore = scoreValue;
        }
    }

    private void SaveTotalTime()
    {
        string totalTimeKey = GetLevelKey("TotalTime");
        float totalTime = PlayerPrefs.GetFloat(totalTimeKey, 0f);
        totalTime += gameTime;
        PlayerPrefs.SetFloat(totalTimeKey, totalTime);
        PlayerPrefs.Save(); 
    }

    public void OpenEndScreen()
    {
        Time.timeScale = 0;
        isGameOver = true;

        CheckForHighscore(scoreValue);
        SaveTotalTime();

        int finalHighscore = PlayerPrefs.GetInt(GetLevelKey("Highscore"), 0);

        gameOverScreen.SetActive(true);

        yourScoreText.text = "Score: " + scoreValue.ToString("#,#");
        yourHighscore.text = "Highscore: " + finalHighscore.ToString("#,#");

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu(string menuSceneName = "MainMenu")
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Application is quitting...");
        Application.Quit();
    }

    public void ShowMeteorShowerText()
    {
        if (meteorShowerText != null)
        {
            StopAllCoroutines();
            StartCoroutine(MeteorEventRoutine());
        }
    }

    private System.Collections.IEnumerator MeteorEventRoutine()
    {
        meteorShowerText.text = "METEOR SHOWER";
        meteorShowerText.gameObject.SetActive(true);

        yield return new WaitForSeconds(meteorTextDuration);
        meteorShowerText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        if (cameraScroll != null && cameraScroll.enableShake)
        {
            cameraScroll.ShakeCamera();
        }
    }

    public void ToggleCameraShake(bool enabled)
    {
        if (cameraScroll != null)
        {
            cameraScroll.enableShake = enabled;
        }

        PlayerPrefs.SetInt("CameraShake", enabled ? 1 : 0);
        PlayerPrefs.Save();

        if (cameraScroll != null && !enabled)
        {
            cameraScroll.StopShake();
        }
    }

    public void UpdateHPHearts(int currentHP)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            if (i < currentHP)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

        if (currentHP <= 0)
        {
            OpenEndScreen();
        }
    }
}