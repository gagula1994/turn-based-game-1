using UnityEngine;

//Script has a bad name. It's not used only for delete confirmation panel.
public class DeletePanelScript : MonoBehaviour
{
    public GameObject panel;
    public GameObject loadPanel;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    public void CloseDeletePanel()
    {
        loadPanel.GetComponent<CanvasGroup>().interactable = true;
        panel.SetActive(false);
    }

    public void OpenDeletePanel()
    {
        panel.SetActive(true);
        loadPanel.GetComponent<CanvasGroup>().interactable = false;
    }
}
