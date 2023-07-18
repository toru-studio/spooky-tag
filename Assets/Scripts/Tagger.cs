using System;
using System.Data.Common;

using UnityEngine;

public abstract class Tagger : MonoBehaviour
{
    protected Vector3 moveDirection;


    public float moveSpeed;
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
        rigidbody.freezeRotation = true;
    }

    protected void Update()
    {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight, ground);
        rigidbody.drag = isOnGround ? gDrag : aDrag;
    }


    //Moves rigidbody by adding force in the direction of moveDirection
    protected void Move(float inputV, float inputH)
    {
        if (canMove)
        {
            moveDirection = orientation.forward * inputV + orientation.right * inputH;
            rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
    }
   protected void Jump()
   {
       moveDirection += Vector3.up * jumpHeight;
       rigidbody.AddForce(moveDirection, ForceMode.Impulse);
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
        transform.position = new Vector3(curPos.x, pos.y - 2,curPos.z);
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
    
    public void beginVault()
    {
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