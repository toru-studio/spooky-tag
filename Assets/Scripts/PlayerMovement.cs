using UnityEngine;

public class PlayerMovement : Tagger
{

    // Horizontal and Vertical inputs
    float inputH;
    float inputV;
    private CameraController camera;

    [Header("KeyBinds")] public KeyCode jumpKey = KeyCode.Space;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        camera = gameObject.GetComponent<CameraController>();
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
        if (Input.GetKey(jumpKey)  && isOnGround)
        {
            Jump();
        }
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