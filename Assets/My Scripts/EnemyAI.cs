using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject player;
    public GameObject movementTarget;
 //   public Animator enemyAnimator;
    [HideInInspector]
    public GameObject miniMapComp;
    public float followRange;
    public float moveSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 3f;
    public float distanceToPlayer, attackdist;
    float playertocrate;
    public int enemyarea;
    [SerializeField] float distanceToCrate;
    [SerializeField] bool Attacked;
    Transform crate;
    Vector3 randomPosition;


    private bool isFollowingPlayer = false;


    [Header("Enemy variables")]
    PlayerAttack _playerScript;
    

    Clock clock;
    //PlayerAttack plyerattck;



    void Start()
    {
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Sound");
        miniMapComp = GameObject.Find("MiniMapComponent");
        _playerScript = player.GetComponent<PlayerAttack>();
        crate = transform.parent;
        agent = GetComponent<NavMeshAgent>();
        clock = FindObjectOfType<Clock>();
        //enemyHealthTxt.text=enemyHealth.ToString();
        
        
        //rb=GetComponent<Rigidbody>();
    }
    private void Update()
    {
        playertocrate = Vector3.Distance(player.transform.position, crate.position);
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        distanceToCrate = Vector3.Distance(transform.position, crate.position);
        if (distanceToPlayer <= followRange && distanceToCrate<20 && playertocrate < 20)
        {
            isFollowingPlayer = true;
            transform.LookAt(player.transform);
            agent.SetDestination(player.transform.position);
            //enemyAnimator.SetFloat("speed",0.2f);
            this.GetComponent<Animation>().CrossFade("Running");
            agent.speed = runSpeed;
            this.GetComponent<AudioSource>().enabled = true;
            // Set animation parameters

            if (distanceToPlayer < attackdist)
            {
                agent.SetDestination(transform.position);
                transform.LookAt(player.transform);
                agent.speed = 0;
                //this.GetComponent<Animation>().CrossFade("Idle");
                this.GetComponent<Animation>().CrossFade("Attack");
                _playerScript.CallEnemyAttack();
                
            }
            else
            {
                Attacked = false;
                //enemyAnimator.SetBool("attack",false);
            }
        }
        else
        {
            this.GetComponent<AudioSource>().enabled = false;
            isFollowingPlayer = false;
            if (!agent.hasPath || agent.remainingDistance < 0.5f)
            {
                MoveToRandomTarget();
            }
        }

        // Rotate towards the movement direction
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void MoveToRandomTarget()
    {
        // randomPosition = movementTarget.transform.position + Random.insideUnitSphere * 10f;
        randomPosition = movementTarget.transform.position + Random.insideUnitSphere * enemyarea;
        NavMeshHit hit;
        agent.speed = moveSpeed;
        if (NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas))
        {
            this.GetComponent<Animation>().CrossFade("Walk");
            agent.SetDestination(hit.position);
        }
    }



    private void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.red;

        // Draw a sphere at the random position
        Gizmos.DrawWireSphere(randomPosition, 1.0f);
    }
}