#pragma warning disable 0618 
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private Button level2Button;
    [SerializeField] private GameObject lockImage;

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0);

        if (unlocked == 1)
        {
            level2Button.interactable = true;
            lockImage.SetActive(false);
        }
        else
        {
            level2Button.interactable = false;
            lockImage.SetActive(true);
        }
    }
}
