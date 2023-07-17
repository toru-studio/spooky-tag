using UnityEngine;

public class Tagger : MonoBehaviour
{
    protected Vector3 moveDirection;
    public float moveSpeed;


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

    //Moves rigidbody by adding force in the direction of moveDirection
    protected void Move(float inputV, float inputH)
    {
        moveDirection = orientation.forward * inputV + orientation.right * inputH;
        _rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
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