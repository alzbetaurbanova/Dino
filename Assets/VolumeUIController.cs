using UnityEngine;

public class VolumeUIController : MonoBehaviour
{
    [SerializeField] private GameObject bgSettings;
    [SerializeField] private AudioSource musicAudioSource; // nastav v Inspector
    public static bool isMenuOpen = false;

    void Start()
    {
        bgSettings.SetActive(false);
        isMenuOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bgSettings.activeSelf)
                CloseBGSettings();
            else
                OpenBGSettings();
        }
    }

    public void OpenBGSettings()
    {
        bgSettings.SetActive(true);
        isMenuOpen = true;
        Time.timeScale = 0f;
        //musicAudioSource.Pause();
    }

    public void CloseBGSettings()
    {
        bgSettings.SetActive(false);
        isMenuOpen = false;
        Time.timeScale = 1f;
        //musicAudioSource.UnPause();
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game called!");
        Application.Quit();
    }
}
