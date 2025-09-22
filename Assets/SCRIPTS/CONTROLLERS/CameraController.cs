using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensibility;
    public float smoothness;

    public Vector2 mouseScaledPos;
    public Vector2 smoothedCam;
    public Vector2 camPos;

    public float maxVerticalAngle;
    public float minVerticalAngle;

    public Transform player;


    void Start()
    {
        if (player == null)
        {
            player = transform.parent;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    public void RotateCamera()
    {
        mouseScaledPos = Vector2.Scale(InputController.Instance.MousePos(), Vector2.one * sensibility);
        smoothedCam = Vector2.Lerp(smoothedCam, mouseScaledPos, 1 / smoothness); 
        camPos += smoothedCam;

        camPos.y = Mathf.Clamp(camPos.y, minVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.AngleAxis(-camPos.y, Vector3.right);
        player.localRotation = Quaternion.AngleAxis(camPos.x, Vector3.up);
    }
}
