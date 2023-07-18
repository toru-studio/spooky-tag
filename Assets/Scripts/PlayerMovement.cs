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