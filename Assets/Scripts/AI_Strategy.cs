using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Strategy : Tagger
{
    public Transform player;
    private NavMeshAgent agent;
    private bool canRotate;

    void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        canRotate = true;
        isSprinting = true;
    }

    void Update()
    {
        base.Update();
        agent.destination = player.position;
    }

    private void FixedUpdate()
    {
        if (canRotate){
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
    }

    protected override void DisableComponents()
    {
        canRotate = false;
        agent.updatePosition = false;
    }

    protected override void EnableComponents()
    {
        canRotate = true;
        agent.updatePosition = true;
    }
}
