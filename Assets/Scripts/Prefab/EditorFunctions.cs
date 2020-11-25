using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class EditorFunctions : EditorWindow
{
    [MenuItem("Window/EditorFunctions")]
    public static void ShowWindow()
    {
        GetWindow<EditorFunctions>("EditorFunctions");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Destroy Current BuildingsUI"))
        {
            GameObject.Find("BuildingsUI").GetComponent<GeneratePrefab>().DestroyPrefab();
        }

        if (GUILayout.Button("Create BuildingsUI"))
        {
            GameObject.Find("BuildingsUI").GetComponent<GeneratePrefab>().GeneratePrefabs("BuildingUI");
        }

        if (GUILayout.Button("Destroy Current SavesUI"))
        {
            GameObject.Find("SavesUI").GetComponent<GeneratePrefab>().DestroyPrefab();
        }

        if (GUILayout.Button("Create SavesUI"))
        {
            GameObject.Find("SavesUI").GetComponent<GeneratePrefab>().GeneratePrefabs("SaveUI");
        }
    }
}
#endif