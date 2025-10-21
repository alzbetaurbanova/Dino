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

    public void AddScore()
    {
        scoreValue++;
        scoreText.text = scoreValue.ToString("#,#");
    }

    public void UpdateAmmoInfo(int currentBullets, int maxBullets)
    {
        ammoText.text = currentBullets + "/" + maxBullets;
    }

    public void CheckForHighscore(int scoreValue)
    {
        int bestScore = PlayerPrefs.GetInt("Highscore", 0);

        if (scoreValue > bestScore)
        {
            PlayerPrefs.SetInt("Highscore", scoreValue);
            PlayerPrefs.Save();
        }
    }

    public void OpenEndScreen()
    {
        Time.timeScale = 0;
        isGameOver = true;

        CheckForHighscore(scoreValue);
        int finalHighscore = PlayerPrefs.GetInt("Highscore", 0);

        gameOverScreen.SetActive(true);

        yourScoreText.text = "Score: " + scoreValue.ToString("#,#");
        yourHighscore.text = "Highscore: " + finalHighscore.ToString("#,#");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Game is quitting...");
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