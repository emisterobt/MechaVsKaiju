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

    public bool lockMainCamera = false;
    public bool inCombat = false;

    private Quaternion iniCamPos;

    [Header("Cameras")]
    //public Camera sideCamera;
    public Camera mainCamera;
    public Transform rotator;

    private PlayerMovement pM;

    void Start()
    {
        if (player == null)
        {
            player = transform.parent;
            pM = player.GetComponent<PlayerMovement>();
        }
        Cursor.lockState = CursorLockMode.Locked;
        iniCamPos = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (pM.died == true)
        {
            LockCameraRotation();
            //OnCombat();
        }
        else if (lockMainCamera == true || GameManager.Instance.isInPause == true)
        {
            LockCameraRotation();
            ChangeToSpecificAngle(new Vector3(12f, -134f, -0.1f));

        }
        else
        {
            ResetRotator();
            OutOfCombat();
            RotateCamera();
        }

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

    public void OnCombat()
    {
        //sideCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }

    public void OutOfCombat()
    {
        mainCamera.gameObject.SetActive(true);
        //sideCamera.gameObject.SetActive(false);
    }

    public void LockCameraRotation()
    {
        transform.localRotation = Quaternion.Euler(iniCamPos.x, transform.localRotation.y, transform.localRotation.z);
        player.localRotation = player.localRotation;

    }

    public void ChangeToSpecificAngle(Vector3 newAngle)
    {
        rotator.localRotation = Quaternion.Euler(newAngle);
    }

    public void ResetRotator()
    {
        rotator.localRotation = Quaternion.Euler(0, 0, 0);
    }

}
