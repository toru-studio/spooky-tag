using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity;
    public Transform orientation;

    private float rotationY;
    private float rotationX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}