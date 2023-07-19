using System;
using System.Data.Common;
using UnityEngine;

public abstract class Tagger : MonoBehaviour
{
    protected Vector3 moveDirection;


    public float moveSpeed;
    public float sprintSpeed;
    protected bool sprinting;
    public float playerHeight;
    public LayerMask ground;
    protected bool isOnGround;
    public float gDrag;
    public float aDrag;
    public float jumpHeight;

    public Transform orientation;
    public CameraController camera;
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
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, ground);
        rigidbody.drag = isOnGround ? gDrag : aDrag;
    }


    //Moves rigidbody by adding force in the direction of moveDirection
    protected void Move(float inputV, float inputH)
    {
        if (!canMove) return;
        moveDirection = orientation.forward * inputV + orientation.right * inputH;
        var speed = sprinting ? sprintSpeed : moveSpeed;
        rigidbody.AddForce(moveDirection.normalized * speed, ForceMode.Force);
    }

    protected void Jump()
    {
        moveDirection += Vector3.up * (jumpHeight / 100f);
        rigidbody.AddForce(moveDirection, ForceMode.Impulse);
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