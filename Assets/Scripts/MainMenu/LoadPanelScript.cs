using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelScript : MonoBehaviour
{
    public GameObject panel;
    public GameObject savesUI;

    DirectoryInfo saveDir;
    FileInfo[] saveInfo;
    GameObject[] savesToggle;
    

    void Start()
    {
        panel.SetActive(false);
    }

    public void OpenLoadPanel()
    {
        panel.SetActive(true);

        if (Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            saveDir = new DirectoryInfo(Application.persistentDataPath + "/saves/");
            saveInfo = saveDir.GetFiles("*.game");
            if (saveInfo.Length != 0)
            {
                savesUI.GetComponent<GeneratePrefab>().GeneratePrefabs("SavesUI", saveInfo.Length);
                savesToggle = GameObject.FindGameObjectsWithTag("SaveToggle");

                if (savesToggle != null)
                {
                    if (savesToggle.Length > 0)
                    {
                        for (int i = 0; i < savesToggle.Length; i++)
                        {
                            savesToggle[i].GetComponentInChildren<Text>().text = saveInfo[i].Name;
                        }
                    }
                }
            }
        }
    }

    public void CloseLoadPanel()
    {
        savesToggle = null;
        savesUI.GetComponent<GeneratePrefab>().DestroyPrefab();
        panel.SetActive(false);
    }
}
