using UnityEngine;

public class EscScript : MonoBehaviour
{
    public GameObject panel;
    public MainCameraScript mCamera;

    bool panelActive = false;
    float temp;

    void Start()
    {
        panel.SetActive(panelActive);
        temp = mCamera.zoomSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (panelActive)
            {
                panelActive = false;
                mCamera.zoomSpeed = temp;
            }
            else
            {
                panelActive = true;
                mCamera.zoomSpeed = 0;
            }
            panel.SetActive(panelActive);
        }
    }
}
