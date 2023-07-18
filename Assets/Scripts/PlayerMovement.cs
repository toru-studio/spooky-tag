using UnityEngine;

public class PlayerMovement : Tagger
{
    // Horizontal and Vertical inputs
    float inputH;
    float inputV;

    [Header("KeyBinds")] public KeyCode jumpKey = KeyCode.Space;

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
        base.Move(inputV, inputH);
    }

    private void Inputs()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && isOnGround)
        {
            Jump();
        }
        sprinting = Input.GetKey(sprintKey);
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