#pragma warning disable 0618
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectionPanel;

    public void Start()
    {
        levelSelectionPanel.SetActive(false);
    }

    public void ShowLevelSelection()
    {
        levelSelectionPanel.SetActive(true); // zobrazí panel
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
