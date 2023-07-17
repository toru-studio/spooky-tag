using System.Data.Common;
using UnityEngine;

public abstract class Tagger : MonoBehaviour
{
    protected Vector3 moveDirection;
    public float moveSpeed;


    public Transform orientation;
    protected Rigidbody rigidbody;
    protected Animator animator;
    private Vector3 nextAnimPosition;
    private string curAnim;

    public bool canMove = true;

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponentInChildren<CharacterCollection>().GetComponent<Animator>();
        moveDirection = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        curAnim = "empty";
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

    protected void Update()
    {
        // Checks the current animation
        switch (curAnim)
        {
            case "empty":
                break;
            case "climb":
                // If the animation is in transition to the Empty state end Climb
                if (animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).IsName("Empty"))
                {
                    endClimb();
                }
                break;
        }
        
    }

    public void beginClimb(Vector3 pos)
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
        curAnim = "climb";
        // Save the target position
        nextAnimPosition = pos;
    }

    private void endClimb()
    {
        // Teleport the player to the expected position
        transform.position = nextAnimPosition;
        // Enable Movement and Components
        canMove = true;
        EnableComponents();
        // Update animation state
        curAnim = "empty";
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