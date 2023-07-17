using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagger : MonoBehaviour
{
    protected CharacterController _characterController;

    protected Vector3 moveDirection;

    protected Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
