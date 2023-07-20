using UnityEngine;

public abstract class Tagger : MonoBehaviour
{
    private Vector3 moveDirection;

    [Header("States")] public MoveState currentState;
    protected bool isSprinting;
    protected bool isOnGround;
    protected bool isCrouching;
    protected bool isSliding;

    [Header("Speeds")] private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float airMultiplier;

    [Header("Slide")] public float slideTimer;
    public float slideForce;
    public float maxSlideTime;

    [Header("Slopes")] public float maxAngle;
    private RaycastHit slopeHit;

    [Header("Jumping")] 
    public bool canJump = true;
    public float jumpHeight;
    public float jumpLimit;


    [Header("Heights")] public float playerHeight;
    protected float playerHeightStartScale;
    public float crouchHeightScale;

    [Header("Drag Control")] public float gDrag;
    public float aDrag;

    [Header("Misc")] public LayerMask ground;
    public Transform orientation;
    public bool canMove = true;
    public CameraController camera;

    protected Rigidbody rigidbody;
    protected Animator animator;
    private Vector3 nextAnimPosition;


    public enum MoveState
    {
        inAir,
        inSprint,
        inWalk,
        inCrouch,
        onSlope,
        inSlide
    }


    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponentInChildren<CharacterCollection>().GetComponent<Animator>();
        moveDirection = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        playerHeightStartScale = transform.localScale.y;
        resetJump();
    }

    protected void Update()
    {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        rigidbody.drag = isOnGround ? gDrag : aDrag;
        changeState();
    }

    private void changeState()
    {
        if (isSliding)
        {
            currentState = MoveState.inSlide;
        }
        else if (isCrouching)
        {
            currentState = MoveState.inCrouch;
            moveSpeed = crouchSpeed;
        }
        else if (isOnGround && isSprinting)
        {
            currentState = MoveState.inSprint;
            moveSpeed = sprintSpeed;
        }

        else if (isOnGround)
        {
            currentState = MoveState.inWalk;
            moveSpeed = walkSpeed;
        }
        else if (onSlope())
        {
            currentState = MoveState.onSlope;
        }
        else
        {
            currentState = MoveState.inAir;
        }
    }

    //Moves rigidbody by adding force in the direction of moveDirection
    protected void Move(float inputV, float inputH)
    {
        if (canMove)
        {
            moveDirection = orientation.forward * inputV + orientation.right * inputH;
            if (onSlope())
            {
                rigidbody.AddForce(getSlopeMove() * (moveSpeed * 20f), ForceMode.Force);
                if (rigidbody.velocity.y > 0)
                {
                    rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
            else
                switch (isOnGround)
                {
                    case true:
                        rigidbody.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
                        break;
                    case false:
                        rigidbody.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier),
                            ForceMode.Force);
                        break;
                }
        }
    }

    protected void ChangeScale(float scale)
    {
        transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
    }

    protected void Jump()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        rigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    protected void resetJump()
    {
        canJump = true;
        
    }


    private bool onSlope()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f)) return false;
        var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        return angle < maxAngle && angle != 0;

    }

    private Vector3 getSlopeMove()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    protected void Sliding(float inputV, float inputH)
    {
        var inputDirection = orientation.forward * inputV + orientation.right * inputH;

        if (!onSlope() || rigidbody.velocity.y > -0.1f)
        {
            rigidbody.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            rigidbody.AddForce(getSlopeMove() * slideForce, ForceMode.Force);
        }
        if (slideTimer <= 0)
        {
            stopSlide();
        }

    }

    protected void stopSlide()
    {
        isSliding = false;
        ChangeScale(playerHeightStartScale);
    }

    protected void startSlide()
    {
        isSliding = true;
        rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        ChangeScale(crouchHeightScale);
        slideTimer = maxSlideTime;
    }

    protected void speedLimiter()
    {
        var velocity = rigidbody.velocity;
        var velocityLimit = new Vector3(velocity.x, 0f, velocity.z);
        var limitVelocity = velocityLimit.normalized * moveSpeed;
        if (onSlope())
        {
            if (rigidbody.velocity.magnitude > moveSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            if (!(velocityLimit.magnitude > moveSpeed)) return;
            rigidbody.velocity = new Vector3(limitVelocity.x, rigidbody.velocity.y, limitVelocity.z);
        }
    }

    public void beginClimb(Vector3 pos)
    {
        // Clear Velocity
        moveDirection = Vector3.zero;
        // Disable Movement and Components
        canMove = false;
        DisableComponents();
        // I would like this to be abstracted but for now this will do
        if (camera != null)
        {
            Vector3 cameraLookDir = pos - camera.transform.position;
            cameraLookDir.y = 0.0f;
            Quaternion rotation = Quaternion.LookRotation(cameraLookDir);
            camera.transform.rotation = rotation;
            transform.rotation = rotation;
        }

        // Trigger the animation and set current state
        animator.SetTrigger("climb");
        // Save the target position
        nextAnimPosition = pos;
    }

    public void endClimb()
    {
        // Teleport the player to the expected position
        transform.position = nextAnimPosition;
        // Enable Movement and Components
        canMove = true;
        EnableComponents();
    }

    public void beginVault(Vector3 pos)
    {
        moveDirection = Vector3.zero;

        canMove = false;
        DisableComponents();
        // I would like this to be abstracted but for now this will do
        if (camera != null)
        {
            Vector3 cameraLookDir = pos - camera.transform.position;
            cameraLookDir.y = 0.0f;
            Quaternion rotation = Quaternion.LookRotation(cameraLookDir);
            camera.transform.rotation = rotation;
            transform.rotation = rotation;
        }

        animator.SetTrigger("vault");

        nextAnimPosition = pos;
    }

    public void endVault()
    {
        // Teleport the player to the expected position
        transform.position = nextAnimPosition;
        // Enable Movement and Components
        canMove = true;
        EnableComponents();
    }

    public void beginSlide()
    {
    }

    // Disables any components
    // NOTE: Could be used to disables the AI's tagging and attacking abilities
    protected abstract void DisableComponents();

    // Enables any components
    // NOTE: Could be sued to enable the AI's tagging and attacking abilities
    protected abstract void EnableComponents();
}