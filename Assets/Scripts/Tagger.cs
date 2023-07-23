using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Jumping")] public bool canJump = true;
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

    public BoxCollider boxCollider;
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
        inSlide,
        inAnimation
    }


    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponentInChildren<CharacterCollection>().GetComponent<Animator>();
        moveDirection = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        playerHeightStartScale = boxCollider.size.y;
        resetJump();
    }

    protected void Update()
    {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        rigidbody.drag = isOnGround ? gDrag : aDrag;
        changeState();
    }

    // TODO Tidy
    private void changeState()
    {
        if (canMove)
        {
            bool isRunning = false;
            bool inAir = false;
            if (isOnGround)
            {
                animator.SetBool("isJump", !canJump);
            }

            if (rigidbody.velocity.magnitude > 0.2 && isOnGround)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (isSliding)
            {
                currentState = MoveState.inSlide;
            }
            else if (isCrouching && isOnGround)
            {
                currentState = MoveState.inCrouch;
                ChangeScale(playerHeightStartScale - 1, crouchHeightScale, playerHeightStartScale - 1, 0f, 0f, 0f);
                rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                moveSpeed = crouchSpeed;
            }
            else if (isOnGround && isSprinting)
            {
                isRunning = true;
                currentState = MoveState.inSprint;
                moveSpeed = sprintSpeed;
            }
            else if (isOnGround)
            {
                currentState = MoveState.inWalk;
                moveSpeed = walkSpeed;
            }
            // Look into this, it may be obsolete
            else if (onSlope())
            {
                currentState = MoveState.onSlope;
            }
            else
            {
                currentState = MoveState.inAir;
                inAir = true;
            }

            // Set animators
            animator.SetBool("isSliding", isSliding);
            animator.SetBool("inAir", inAir);
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isCrouching", isCrouching);
        }
        else
        {
            currentState = MoveState.inAnimation;
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


    protected void ChangeScale(float sizeX, float sizeY, float sizeZ, float centerX, float centerY, float centerZ)
    {
        boxCollider.size = new Vector3(sizeX, sizeY, sizeZ);
        boxCollider.center = new Vector3(centerX, centerY, centerZ);
    }

    protected void Jump()
    {
        if (isOnGround && canJump)
        {
            canJump = false;
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            rigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            animator.SetTrigger("jump");
            Invoke(nameof(resetJump), jumpLimit);
        }
    }

    private void resetJump()
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
        if (canMove)
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
    }

    protected void stopSlide()
    {
        isSliding = false;
    }

    protected void startSlide()
    {
        isSliding = true;
        rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        ChangeScale(playerHeightStartScale - 1, crouchHeightScale, playerHeightStartScale, 0f, 0F, 1F);
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
        disableMove();

        // Clear Velocity
        moveDirection = Vector3.zero;
        // Disable Movement and Components
        DisableComponents();

        // Teleports player to beginning of climb position
        Vector3 curPos = transform.position;
        transform.position = new Vector3(curPos.x, pos.y - 1.8f, curPos.z);

        if (camera != null)
        {
            Vector3 cameraLookDir = pos - camera.transform.position;
            cameraLookDir.y = 0.0f;
            Quaternion rotation = Quaternion.LookRotation(cameraLookDir);
            camera.rotationX = 0f;
            camera.rotationY = rotation.y > 0.5f ? -rotation.y + 0.5f : rotation.y;
            camera.rotationY *= 360;
            transform.rotation = rotation;
        }

        // Trigger the animation and set current state
        animator.SetTrigger("climb");
        // Save the target position
        nextAnimPosition = pos;
    }

    private void enableMove()
    {
        canMove = true;
        rigidbody.useGravity = true;
    }

    private void disableMove()
    {
        canMove = false;
        rigidbody.useGravity = false;
    }

    public void endClimb()
    {
        EnableComponents();
        enableMove();
        // Teleport the player to the expected position
        transform.position = nextAnimPosition;
        // Enable Movement and Components
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
            camera.rotationX = 0f;
            camera.rotationY = rotation.y > 0.5f ? -rotation.y + 0.5f : rotation.y;
            camera.rotationY *= 360;
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
        EnableComponents();
        canMove = true;
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