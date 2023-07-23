using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Strategy : Tagger
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    void Update()
    {
        base.Update();
        agent.destination = player.position;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    protected override void DisableComponents()
    {
        agent.updatePosition = false;
    }

    protected override void EnableComponents()
    {
        agent.updatePosition = true;
    }
}
