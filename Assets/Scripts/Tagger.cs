using UnityEngine;

public class Tagger : MonoBehaviour
{
    protected Vector3 moveDirection;
    public float moveSpeed;


    public Transform orientation;
    protected Rigidbody rigidbody;
    protected Animator animator;

    public bool canMove = true;

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>();
        moveDirection = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
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

    public void beginClimb(Vector3 pos)
    {
        Debug.Log("CLIMBING");
        moveDirection = Vector3.zero;
        canMove = false;
        gameObject.transform.position = pos;
        canMove = true;
    }

    public void beginVault()
    {
        
    }

    public void beginSlide()
    {
        
    }
}