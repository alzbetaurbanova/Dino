#pragma warning disable 0618 //vypne upozornenia
using UnityEditor;
using UnityEngine;

public class MissingScriptFinder : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts in Scene")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MissingScriptFinder), false, "Missing Script Finder");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindMissingScripts();
        }
    }

    private static void FindMissingScripts()
    {
        // Tu zmena metódy podľa nového API
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int count = 0;

        foreach (var go in allObjects)
        {
            var components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogWarning($"Missing script found in GameObject: '{go.name}'", go);
                    count++;
                }
            }
        }

        Debug.Log($"Total missing scripts found: {count}");
    }
}
