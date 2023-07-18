using UnityEngine;

public class PlayerMovement : Tagger
{
    // Horizontal and Vertical inputs
    float inputH;
    float inputV;
    public CameraController camera;

    [Header("KeyBinds")] public KeyCode jumpKey = KeyCode.Space;

    public KeyCode crouchKey = KeyCode.LeftControl;

    public KeyCode sprintKey = KeyCode.LeftShift;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        Inputs();
    }


    private void FixedUpdate()
    {
        Move(inputV, inputH);
    }

    private void Inputs()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && isOnGround)
        {
            Jump();
        }

        isSprinting = Input.GetKey(sprintKey);

        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchHeightScale, transform.localScale.z);
            rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            print("Crouch");
        }

        if (Input.GetKeyUp(crouchKey))
        {
            isCrouching = false;
            transform.localScale = new Vector3(transform.localScale.x, playerHeightStartScale, transform.localScale.z);

            print("uncrouch");
        }

        speedLimiter();
    }

    // Disables the camera
    protected override void DisableComponents()
    {
        camera.enabled = false;
    }

    // Enables the camera
    protected override void EnableComponents()
    {
        camera.enabled = true;
    }
}