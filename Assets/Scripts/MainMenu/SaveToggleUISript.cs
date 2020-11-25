using UnityEngine;
using UnityEngine.UI;

public class SaveToggleUISript : MonoBehaviour
{
    public Button delete;
    public Button load;

    GameObject[] savesToggle;
    int toggled = 0;

    void Update()
    {
        savesToggle = GameObject.FindGameObjectsWithTag("SaveToggle");
        if (savesToggle != null)
        {
            if (savesToggle.Length > 0)
            {
                foreach (GameObject save in savesToggle)
                {
                    if (save.GetComponent<Toggle>().isOn)
                    {
                        toggled++;
                    }
                }
            }
        }

        savesToggle = null;

        switch (toggled)
        {
            case 0:
                delete.interactable = false;
                load.interactable = false;
                break;
            case 1:
                delete.interactable = true;
                load.interactable = true;
                break;
            default:
                delete.interactable = true;
                load.interactable = false;
                break;
        }

        toggled = 0;
    }
}
