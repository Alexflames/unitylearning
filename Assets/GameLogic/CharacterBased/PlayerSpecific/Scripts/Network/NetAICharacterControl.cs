﻿using System;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Networking;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class NetAICharacterControl : NetworkBehaviour
{
	public UnityEngine.AI.NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
	public ThirdPersonCharacter character { get; private set; } // the character we are controlling
	public Transform target;// target to aim for
	public GameObject destinationMark;
	public RaycastHit hit;
    private GameObject m_destMark;

	private void Start()
	{
		// get the components on the object we need ( should not be null due to require component so no need to check )
		agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
		character = GetComponent<ThirdPersonCharacter>();
		if (!isLocalPlayer)
		{
			agent.enabled = false;
		}
		

		agent.updateRotation = false;
		agent.updatePosition = true;
	}


	private void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (Input.GetButton("Walk"))
		{

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				agent.destination = hit.point;
                if (!m_destMark)
                {
                    m_destMark = GameObject.Instantiate(destinationMark, hit.point, new Quaternion());
                }
			}
		}

		if (target != null)
			agent.SetDestination(target.position);

		if (agent.remainingDistance > agent.stoppingDistance)
			character.Move(agent.desiredVelocity, false, false);
		else
			character.Move(Vector3.zero, false, false);
	}


	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}