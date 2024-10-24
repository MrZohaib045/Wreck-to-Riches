using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class randommove : MonoBehaviour
{
	public static int randScrptControlr;
	private NavMeshAgent m_Agent;
	private void Start()
	{
		randScrptControlr = 1;
	}
	void FixedUpdate()
	{

		if (!Agent.hasPath && randScrptControlr == 1)
		{
			RandomBot();
		}
	}

	void RandomBot()
	{
		Vector3 randomDirection = Random.insideUnitSphere * 50;
		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, 50, 1);
		Vector3 finalPosition = hit.position;
		if(Agent.gameObject != null) Agent.SetDestination(finalPosition);
	}
	private NavMeshAgent Agent
	{
		get
		{
			if (m_Agent == null)
			{
				m_Agent = GetComponent<NavMeshAgent>();
			}
			return m_Agent;
		}
	}
}
