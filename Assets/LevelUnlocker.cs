using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private int totalLevels = 6;

    void Start()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            string levelKey = "Level" + i + "Unlocked";
            bool isUnlocked = PlayerPrefs.GetInt(levelKey, i == 1 ? 1 : 0) == 1;

            GameObject btnObj = GameObject.Find("Level" + i + "Button");

            if (btnObj != null)
            {
                Button btn = btnObj.GetComponent<Button>();
                Transform lockObj = btnObj.transform.Find("Lock");

                if (btn != null)
                    btn.interactable = isUnlocked;

                if (lockObj != null)
                    lockObj.gameObject.SetActive(!isUnlocked);
            }
            else
            {
                Debug.LogWarning("Button for Level " + i + " not found!");
            }
        }
    }
}
