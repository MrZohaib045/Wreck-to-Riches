using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class Shooters : MonoBehaviour
{
    public float fov, runSpeed;
    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {

            if (DistFromEnmyToPlayer() <= 20f)
            {
                GetComponent<Animation>().CrossFade("Running");
                GetComponent<randommove>().enabled = false;
                Vector3 opposDir = DirFromEnemyToPlayer().normalized;
                transform.rotation = EnemyLookTowardPlayer(opposDir);
                navMeshAgent.enabled = false;
                rb.MovePosition(transform.position + opposDir * Time.deltaTime * runSpeed);
            }
            else
            {
                navMeshAgent.enabled = true;
                GetComponent<randommove>().enabled = true;
                GetComponent<Animation>().CrossFade("Walk");
            }
    }
    public Vector3 DirFromEnemyToPlayer()
    {
        Vector3 enmyToPlayeDir = ThirdPersonCharacter.mainPlayer.transform.position - transform.position;
        return enmyToPlayeDir;
    }
    public float DistFromEnmyToPlayer()
    {
        float dis = Vector3.Distance(transform.position, ThirdPersonCharacter.mainPlayer.transform.position);
        return dis;
    }
    public Quaternion EnemyLookTowardPlayer(Vector3 getDir)
    {
        Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(getDir), Time.deltaTime* 3f);
        return look;
    }
}