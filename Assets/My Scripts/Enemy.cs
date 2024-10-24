using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using static ControlFreak2.TouchControl;

public class Enemy : MonoBehaviour
{
    public GameObject Ragdol;
    public Image enemyhealth;
	public float  fardist, neardist, runSpeed, walkSpeed;
	public int  relaxingConter;
	NavMeshAgent navmesh;
	int randomwalk;
	PlayerAttack plyerattck;
	void Start()
	{
        
		plyerattck = FindObjectOfType<PlayerAttack>();
		navmesh = GetComponent<NavMeshAgent>();
	}
    void Update()
    {
        if (Vector3.Distance(transform.position, ThirdPersonCharacter.mainPlayer.transform.position) <= fardist)
        {
            if (Vector3.Distance(transform.position, ThirdPersonCharacter.mainPlayer.transform.position) > neardist)
            {
                randomwalk = 0;
                GetComponent<randommove>().enabled = false;
                navmesh.enabled = true;
                navmesh.speed = runSpeed;
                GetComponent<Animation>().CrossFade("Running");
                navmesh.SetDestination(ThirdPersonCharacter.mainPlayer.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ThirdPersonCharacter.mainPlayer.transform.position - transform.position), Time.deltaTime * 9);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                relaxingConter = 1;
            }
            else if (Vector3.Distance(transform.position, ThirdPersonCharacter.mainPlayer.transform.position) <= neardist)
            {
                randomwalk = 0;
                navmesh.enabled = false;
                GetComponent<randommove>().enabled = false;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ThirdPersonCharacter.mainPlayer.transform.position - transform.position), Time.deltaTime * 9);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                GetComponent<Animation>().CrossFade("Attack");
                plyerattck.CallEnemyAttack();
            }
        }
        if (Vector3.Distance(ThirdPersonCharacter.mainPlayer.transform.position, transform.position) > fardist)
        {
            randomwalk = 1;
            if (relaxingConter == 1)
            {
                relaxingConter = 0;
                PlayingWalkAnim();
            }
        }
    }
    void PlayingWalkAnim()
    {
        if (randomwalk == 1)
        {
            navmesh.enabled = true;
            navmesh.speed = walkSpeed;
            GetComponent<Animation>().CrossFade("Walk");
            GetComponent<randommove>().enabled = true;
            int randomStayTime = Random.Range(10, 15);
            Invoke(nameof(PlayIdleAnim), randomStayTime);
        }
    }
    void PlayIdleAnim()
    {
        if (randomwalk == 1)
        {
            GetComponent<Animation>().CrossFade("Idle");
            navmesh.speed = 0f;
            GetComponent<randommove>().enabled = false;
            int randomStayTime = Random.Range(3, 7);
            Invoke(nameof(PlayingWalkAnim), randomStayTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ThirdPersonCharacter.attackison == true)
        {


            if (collision.collider.gameObject.CompareTag("weapon"))
            {
                Debug.Log("enemy");
                enemyhealth.fillAmount -= 0.4f;
                if (enemyhealth.fillAmount <= 0)
                {
                    Instantiate(Ragdol, transform.position, transform.rotation);
                    Destroy(this.gameObject);
                }
            }
       }
    }
}

