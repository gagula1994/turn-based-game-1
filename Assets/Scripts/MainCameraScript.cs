using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    [Header("Camera movement:")]
    public float speedMouse;
    public float speedKeyboard;
    public float screenEdgeBorder;

    [Header("Camera zoom:")]
    public float zoomSpeed;
    public float zoomIn = 5;
    public float zoomOut = 10;

    [Header("Map size:")]
    public float limitX;
    public float limitY;

    Transform mCamera;

    // Start is called before the first frame update
    void Start()
    {
        mCamera = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.mouseScrollDelta.y != 0) Zoom();
    }

    public void Movement()
    {
        Vector2 move = new Vector2();
        float speed = speedKeyboard;

        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        if (move.x == 0 && move.y == 0)
        {
            Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
            Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
            Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
            Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

            move.x = leftRect.Contains(Input.mousePosition) ? -1 : rightRect.Contains(Input.mousePosition) ? 1 : 0;
            move.y = upRect.Contains(Input.mousePosition) ? 1 : downRect.Contains(Input.mousePosition) ? -1 : 0;

            speed = speedMouse;
        }

        move *= speed * Time.deltaTime;

        mCamera.Translate(move, Space.Self);
        mCamera.position = new Vector3(Mathf.Clamp(mCamera.position.x, -limitX, limitX), Mathf.Clamp(mCamera.position.y, -limitY, limitY), mCamera.position.z);
    }

    public void Zoom()
    {
        Camera.main.orthographicSize = Camera.main.orthographicSize + Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomIn, zoomOut);
    }
}
