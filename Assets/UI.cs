#pragma warning disable 0618
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI : MonoBehaviour
{
    public static UI instance; //pre všetky scripty dostupne
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
    //public int highscore = PlayerPrefs.GetInt("Highscore", 0); --> POTOM ODKOMENTUJ NA HIGHSCORE
    [SerializeField] private CameraScroll cameraScroll;
    [SerializeField] private UnityEngine.UI.Toggle shakeToggle;
    public bool isCameraShakeEnabled = true;

    [SerializeField] private UnityEngine.UI.Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;



    private void Awake()
    {
        instance = this; //pre všetky scripty dostupne

    }
    void Start()
    {
        gameTime = 0f;
        gameOverScreen.SetActive(false);

        // Načítaj uložený stav zo storage a nastav kameru
        cameraScroll.enableShake = PlayerPrefs.GetInt("CameraShake", 1) == 1;

        if (shakeToggle != null)
        {
            // Odstráni event listener, aby nenastalo volanie ToggleCameraShake pri nastavovaní isOn
            shakeToggle.onValueChanged.RemoveAllListeners();

            // Nastaví toggle bez spustenia eventu
            shakeToggle.isOn = cameraScroll.enableShake;

            // Pripni event listener, ktorý bude volať ToggleCameraShake pri zmene UI
            shakeToggle.onValueChanged.AddListener(ToggleCameraShake);
        }
    }



    void Update()
    {
        if (!isGameOver)
        {
            gameTime += Time.deltaTime;
            timerText.text = gameTime.ToString("#,#"); //"#,#" --> int sekundy 
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
        int bestScore = PlayerPrefs.GetInt("Highscore", 0); // 0 = default


        if (scoreValue > bestScore)
        {
            PlayerPrefs.SetInt("Highscore", scoreValue);
            PlayerPrefs.Save(); // ulož do súboru
            //Debug.Log("New Highscore: " + scoreValue);
        }
    }

    public void OpenEndScreen()
    {

        Time.timeScale = 0; //vypne to hru
        gameOverScreen.SetActive(true);
        //tryAgainButton.SetActive(true);
        yourScoreText.text = "Score: " + scoreValue.ToString("#,#");
        yourHighscore.text = "Highscore: " + scoreValue.ToString("#,#"); //highscore bez to string
        isGameOver = true;
        CheckForHighscore(scoreValue);


    }
    public void RestartGame()
    {
        gameTime = 0f;
        isGameOver = false;
        Time.timeScale = 1;

        // Resetni stav hudby
        if (MusicManager.Instance != null)
            MusicManager.Instance.ResetMusicState();

        SceneManager.LoadScene(0);
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

        if (cameraScroll.enableShake)
        {
            cameraScroll.ShakeCamera();
        }
    }

    public void ToggleCameraShake(bool enabled)
    {

        cameraScroll.enableShake = enabled;

        PlayerPrefs.SetInt("CameraShake", enabled ? 1 : 0);
        PlayerPrefs.Save();

    // Debug.Log("KAMERA HALO: " + enabled);

        if (!enabled)
        {
            cameraScroll.StopShake();  // Ak chceš zastaviť shake keď toggle vypneš
        }
    }



    public void UpdateHPHearts(int currentHP)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
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
