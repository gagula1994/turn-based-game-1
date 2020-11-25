using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSaveScript : MonoBehaviour
{
    public Button noButton;
    public Button closeButton;
    public Button loadPanelButton;
    GameObject[] savesToggle;

    public void DeleteSave()
    {
        savesToggle = GameObject.FindGameObjectsWithTag("SaveToggle");

        foreach (GameObject save in savesToggle)
        {
            if (save.GetComponent<Toggle>().isOn)
            {
                File.Delete(Application.persistentDataPath + "/saves/" + save.GetComponentInChildren<Text>().text);
            }
        }

        noButton.onClick.Invoke();
        closeButton.onClick.Invoke();
        loadPanelButton.onClick.Invoke();
    }
}
