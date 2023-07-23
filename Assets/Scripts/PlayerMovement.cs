using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : Tagger
{
    // Horizontal and Vertical inputs
    float inputH;
    float inputV;

    [Header("KeyBinds")] public static KeyCode jumpKey = KeyCode.Space;

    public static KeyCode crouchKey = KeyCode.LeftControl;

    public static KeyCode sprintKey = KeyCode.LeftShift;


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
        if (isSliding)
        {
            Sliding(inputV, inputH);
        }
    }

    private void Inputs()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey))
        {
            Jump();
        }

        isSprinting = Input.GetKey(sprintKey);

        if (Input.GetKeyDown(crouchKey) && isSprinting)
        {
            startSlide();
        }

        if (Input.GetKeyUp(crouchKey) && isSliding)
        {
            stopSlide();
        }

        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = true;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            isCrouching = false;
            ChangeScale(playerHeightStartScale - 1, playerHeightStartScale, playerHeightStartScale - 1, 0f, 0f, 0f, 1);
        }


        speedLimiter();
    }


    // Disables the camera
    protected override void DisableComponents()
    {
        base.camera.enabled = false;
    }

    // Enables the camera
    protected override void EnableComponents()
    {
        base.camera.enabled = true;
    }
}