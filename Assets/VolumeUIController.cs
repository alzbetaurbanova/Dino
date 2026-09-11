using UnityEngine;

public class VolumeUIController : MonoBehaviour
{
    [SerializeField] private GameObject bgSettings;
    public static bool isMenuOpen = false;

    void Start()
    {
        bgSettings.SetActive(false);
        isMenuOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenBGSettings();
    }

    // Both the gear button and Escape call this; a second click closes the panel.
    public void OpenBGSettings()
    {
        if (bgSettings.activeSelf)
        {
            CloseBGSettings();
            return;
        }

        bgSettings.SetActive(true);
        isMenuOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseBGSettings()
    {
        bgSettings.SetActive(false);
        isMenuOpen = false;
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game called!");
        Application.Quit();
    }
}
