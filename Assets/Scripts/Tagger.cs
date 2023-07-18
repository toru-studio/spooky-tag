using System;
using System.Data.Common;
using UnityEngine;

public abstract class Tagger : MonoBehaviour
{
    protected Vector3 moveDirection;


    public float moveSpeed;
    public float sprintSpeed;
    protected bool sprinting;
    public float airMultiplier;
    public float playerHeight;
    public LayerMask ground;
    protected bool isOnGround;
    public float gDrag;
    public float aDrag;
    public float jumpHeight;

    public Transform orientation;
    protected Rigidbody rigidbody;
    protected Animator animator;
    private Vector3 nextAnimPosition;

    public bool canMove = true;

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponentInChildren<CharacterCollection>().GetComponent<Animator>();
        moveDirection = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        rigidbody.drag = isOnGround ? gDrag : aDrag;
    }


    //Moves rigidbody by adding force in the direction of moveDirection
    protected void Move(float inputV, float inputH)
    {
        if (!canMove) return;
        moveDirection = orientation.forward * inputV + orientation.right * inputH;
        var speed = sprinting ? sprintSpeed : moveSpeed;
        var speedAir = isOnGround ? speed : speed * airMultiplier;
        rigidbody.AddForce(moveDirection.normalized * speedAir, ForceMode.Force);
    }

    protected void Jump()
    {
        moveDirection += Vector3.up * (jumpHeight / 100f);
        rigidbody.AddForce(moveDirection, ForceMode.Impulse);
    }

    protected void speedLimiter()
    {
        Vector3 velocityLimit = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        if (!(velocityLimit.magnitude > sprintSpeed)) return;
        Vector3 limitVelocity = velocityLimit.normalized * sprintSpeed;
        rigidbody.velocity = new Vector3(limitVelocity.x, limitVelocity.y, limitVelocity.z);
    }

    public void beginClimb(Vector3 pos, Vector3 dir)
    {
        // Clear Velocity
        moveDirection = Vector3.zero;
        // Disable Movement and Components
        canMove = false;
        DisableComponents();
        // Change the position to current - the difference in height from animating (2)
        Vector3 curPos = transform.position;
        transform.position = new Vector3(curPos.x, pos.y - 2, curPos.z);
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