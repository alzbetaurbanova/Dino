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
    private bool statsSaved = false;

    [Space]
    [SerializeField] private GameObject gameOverScreen;
    private float gameTime = 0f;

    [SerializeField] private CameraScroll cameraScroll;
    [SerializeField] private UnityEngine.UI.Toggle shakeToggle;

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

        if (cameraScroll != null)
        {
            cameraScroll.enableShake = PlayerPrefs.GetInt("CameraShake", 1) == 1;
        }

        if (shakeToggle != null && cameraScroll != null)
        {
            shakeToggle.onValueChanged.RemoveAllListeners();
            shakeToggle.SetIsOnWithoutNotify(cameraScroll.enableShake);
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

    private void SaveRunStats()
    {
        if (statsSaved) return;
        statsSaved = true;

        CheckForHighscore(scoreValue);
        SaveTotalTime();
    }

    public void OpenEndScreen()
    {
        Time.timeScale = 0;
        isGameOver = true;

        SaveRunStats();

        int finalHighscore = PlayerPrefs.GetInt(GetLevelKey("Highscore"), 0);

        gameOverScreen.SetActive(true);

        yourScoreText.text = "Score: " + scoreValue.ToString("#,#");
        yourHighscore.text = "Highscore: " + finalHighscore.ToString("#,#");

    }

    public void RestartGame()
    {
        SaveRunStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu(string menuSceneName = "MainMenu")
    {
        SaveRunStats();
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Application is quitting...");
        SaveRunStats();
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        SaveRunStats();
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
    }
}