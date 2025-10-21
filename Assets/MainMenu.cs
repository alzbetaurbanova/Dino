#pragma warning disable 0618
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<string> knownLevels = new List<string> { "Dino", "Level2", "Level3", "Level4", "Level5", "Level6" };

    [SerializeField] private GameObject levelSelectionPanel;
    [SerializeField] private GameObject statsPanel;

    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI meteorsText;
    [SerializeField] private TextMeshProUGUI totalPlaytimeText;

    public void Start()
    {
        HideAllPanels();
        LoadAndDisplayStats();
    }

    private void HideAllPanels()
    {
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(false);
        }
        if (statsPanel != null)
        {
            statsPanel.SetActive(false);
        }
    }

    public void ShowLevelSelection()
    {
        if (levelSelectionPanel == null) return;

        bool isAlreadyActive = levelSelectionPanel.activeSelf;

        HideAllPanels();

        if (!isAlreadyActive)
        {
            levelSelectionPanel.SetActive(true);
        }
    }

    public void ShowStats()
    {
        if (statsPanel == null) return;

        bool isAlreadyActive = statsPanel.activeSelf;

        HideAllPanels();

        if (!isAlreadyActive)
        {
            statsPanel.SetActive(true);
            LoadAndDisplayStats();
        }
    }

    private void LoadAndDisplayStats()
    {
        StringBuilder highscoreSb = new StringBuilder();
        StringBuilder meteorsSb = new StringBuilder();
        StringBuilder playtimeSb = new StringBuilder();

        int combinedMeteors = 0;
        float combinedTimeSeconds = 0f;
        int highestHighscore = 0;

        highscoreSb.AppendLine(" ");
        meteorsSb.AppendLine(" ");
        playtimeSb.AppendLine(" ");

        foreach (string levelName in knownLevels)
        {
            int highscore = PlayerPrefs.GetInt(levelName + "_Highscore", 0);
            int meteors = PlayerPrefs.GetInt(levelName + "_TotalMeteors", 0);
            float timeSeconds = PlayerPrefs.GetFloat(levelName + "_TotalTime", 0f);

            combinedMeteors += meteors;
            combinedTimeSeconds += timeSeconds;
            if (highscore > highestHighscore)
            {
                highestHighscore = highscore;
            }

            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timeSeconds);
            string timeFormatted = string.Format("{0:D2}m {1:D2}s", timeSpan.Minutes + (timeSpan.Hours * 60), timeSpan.Seconds);

            highscoreSb.AppendLine($"\n{highscore}");
            meteorsSb.AppendLine($"\n{meteors}");
            playtimeSb.AppendLine($"\n{timeFormatted}");
        }

        highscoreSb.AppendLine("\n\n");
        meteorsSb.AppendLine("\n\n");
        playtimeSb.AppendLine("\n\n");

        System.TimeSpan totalTimeSpan = System.TimeSpan.FromSeconds(combinedTimeSeconds);
        string globalTimeFormatted = string.Format("{0:D2}h {1:D2}m {2:D2}s",
                                                 totalTimeSpan.Hours,
                                                 totalTimeSpan.Minutes,
                                                 totalTimeSpan.Seconds);

        highscoreSb.AppendLine($"{highestHighscore}");
        meteorsSb.AppendLine($"{combinedMeteors}");
        playtimeSb.AppendLine($"{globalTimeFormatted}");

        if (highscoreText != null)
        {
            highscoreText.text = highscoreSb.ToString();
        }
        if (meteorsText != null)
        {
            meteorsText.text = meteorsSb.ToString();
        }
        if (totalPlaytimeText != null)
        {
            totalPlaytimeText.text = playtimeSb.ToString();
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}