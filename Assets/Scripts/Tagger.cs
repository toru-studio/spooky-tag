using System;
using UnityEngine;

public class Tagger : MonoBehaviour
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
    protected Rigidbody _rigidbody;
    protected Animator _animator;


    // Start is called before the first frame update
    protected void Start()
    {
        _animator = GetComponent<Animator>();
        moveDirection = Vector3.zero;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    protected void Update()
    {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight, ground);
        _rigidbody.drag = isOnGround ? gDrag : aDrag;
    }


    //Moves rigidbody by adding force in the direction of moveDirection
    protected void Move(float inputV, float inputH)
    {
        moveDirection = orientation.forward * inputV + orientation.right * inputH;
        _rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

   protected void Jump()
   {
       moveDirection += Vector3.up * jumpHeight;
        _rigidbody.AddForce(moveDirection, ForceMode.Impulse);
    }

    void beginClimb()
    {
    }

    void beginVault()
    {
    }

    void beginSlide()
    {
    }
}