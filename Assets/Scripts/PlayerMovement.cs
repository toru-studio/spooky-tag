using UnityEngine;

public class PlayerMovement : Tagger
{
    bool canMove = true;

    // Horizontal and Vertical inputs
    float inputH;
    float inputV;

    [Header("KeyBinds")] public KeyCode jumpKey = KeyCode.Space;


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
        if (Input.GetKey(jumpKey)  && isOnGround)
        {
            Jump();
        }
    }
}