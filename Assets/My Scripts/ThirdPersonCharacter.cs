using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 12f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;


		
		Rigidbody m_Rigidbody;
		Animator m_Animator;
		bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;

	   public static int wait;
		int run;
        public static bool myanimation,attackison;
		bool axeon;
		[Header("My Code")]
		public static Animation mainPlayer;
		public GameObject sword, axe;
		

		void Start()
		{
			axeon = true;
			run = 0;
			attackison=false;
			mainPlayer = transform.GetChild(0).GetComponent<Animation>();
            myanimation = true;
			//sleepon = false;
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;
			wait = 1;
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;
        }


		public void Move(Vector3 move, bool crouch, bool jump)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();

			// control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				HandleGroundedMovement(crouch, jump);
			}
			else
			{
				HandleAirborneMovement();
			}

			ScaleCapsuleForCrouching(crouch);
			PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}


		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!m_Crouching)
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
				}
			}
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}
            //		if (animatt.death == 0)
            //		{
            if (myanimation == true)
            {
                if (m_IsGrounded && move.magnitude > 0)
                {
                    m_Animator.speed = m_AnimSpeedMultiplier;
                    if (run == 0)
                    {
                        m_MoveSpeedMultiplier = 1.8f;
                        mainPlayer.GetComponent<Animation>().CrossFade("Walk");
                    //    GetComponent<AudioSource>().enabled = true;
                    //    GetComponent<AudioSource>().pitch = 1f;
                    //    mainPlayer.GetComponent<Animation>()["Walk"].speed = 1.5f;
                    }
                    else if (run == 1)
                    {
                        m_MoveSpeedMultiplier = 3f;
                        mainPlayer.GetComponent<Animation>().CrossFade("Running");
                      //  GetComponent<AudioSource>().enabled = true;
                      //  GetComponent<AudioSource>().pitch = 1.8f;
                      //    mainPlayer.GetComponent<Animation>()["Running"].speed = 1.3f;
                    }
                }
                else
                {
                   // this.GetComponent<AudioSource>().enabled = false;
                    mainPlayer.CrossFade("Idle");
                    m_Animator.speed = 1;
                }
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.

            //}
        }

        public void Attack()
        {
            
            if (wait == 1)
            {
                wait = 0;
                StartCoroutine(Attackanim());
            }
        }
        IEnumerator Attackanim()
        {
            m_MoveSpeedMultiplier = 0f;
            attackison = true;
            myanimation = false;
			ThirdPersonUserControl.isRotating = false;
            mainPlayer.CrossFade("Attack");
            mainPlayer.GetComponent<Animation>()["Attack"].speed = 1.2f;
            yield return new WaitForSeconds(mainPlayer["Attack"].length);
			if(PlayerAttack.die==false)
			{
                myanimation = true;
            }  
            ThirdPersonUserControl.isRotating = true;
            wait = 1;
            attackison = false;
        }
        public void Attack2()
        {

            if (wait == 1)
            {
                wait = 0;
                StartCoroutine(Attackanim2());
            }
        }
        IEnumerator Attackanim2()
        {
            m_MoveSpeedMultiplier = 0f;
            attackison = true;
            myanimation = false;
            ThirdPersonUserControl.isRotating = false;
            mainPlayer.CrossFade("Attack2");
            mainPlayer.GetComponent<Animation>()["Attack2"].speed = 1.2f;
            yield return new WaitForSeconds(mainPlayer["Attack2"].length);
			if(PlayerAttack.die==false)
			{
                myanimation = true;
            }  
            ThirdPersonUserControl.isRotating = true;
            wait = 1;
            attackison = false;
        }

        public void Attack3()
        {

            if (wait == 1)
            {
                wait = 0;
                StartCoroutine(Attackanim3());
            }
        }
        IEnumerator Attackanim3()
        {
            m_MoveSpeedMultiplier = 0f;
            attackison = true;
            myanimation = false;
            ThirdPersonUserControl.isRotating = false;
            mainPlayer.CrossFade("Attack3");
            mainPlayer.GetComponent<Animation>()["Attack3"].speed = 1.2f;
            yield return new WaitForSeconds(mainPlayer["Attack3"].length);
            if (PlayerAttack.die == false)
            {
                myanimation = true;
            }
            ThirdPersonUserControl.isRotating = true;
            wait = 1;
            attackison = false;
        }
		public void mypain()
		{
			StartCoroutine(pain());
		}
		IEnumerator pain()
		{
			yield return new WaitForSeconds(1f);
            myanimation = false;
            mainPlayer.CrossFade("Pain");
            yield return new WaitForSeconds(0.3f);
            //   yield return new WaitForSeconds(mainPlayer["Pain"].length);
            if (PlayerAttack.die == false)
            {
                myanimation = true;
            }
        }
        public void running()
        {
            run = 1;
        }
        public void walk()
        {
            run = 0;
        }

		public void changesword()
		{

            if (axeon == true)
            {
                axe.SetActive(false);
				axeon=false;
                if (wait == 1)
                {
                    wait = 0;
                    StartCoroutine(changeswordd());
                }
            }
        }
		
        public void changeaxe()
        {
			if (axeon == false)
			{
				axeon=true;
				sword.SetActive(false);
              //  StartCoroutine(changeweapon());
                if (wait == 1)
                {
                    wait = 0;
                    StartCoroutine(changeaxee());
                }
            }
        }
		IEnumerator changeaxee()
		{
			myanimation = false;
			mainPlayer.CrossFade("Change");
			yield return new WaitForSeconds(mainPlayer["Change"].length);
            axe.SetActive(true);
            if (PlayerAttack.die == false)
            {
                myanimation = true;
            }
            wait = 1;
        }
        IEnumerator changeswordd()
        {
            myanimation = false;
            mainPlayer.CrossFade("Change");
            yield return new WaitForSeconds(mainPlayer["Change"].length);
            sword.SetActive(true);
            if (PlayerAttack.die == false)
            {
                myanimation = true;
            }
            wait = 1;
        }



        void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}

        IEnumerator jumping()
        {
            myanimation = false;
            mainPlayer.GetComponent<Animation>().CrossFade("Jump");
            yield return new WaitForSeconds(mainPlayer["Jump"].length);
            myanimation = true;
        }
        void HandleGroundedMovement(bool crouch, bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				// jump!
				StartCoroutine(jumping());
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
				m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
		}
		
		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_IsGrounded && Time.deltaTime > 0)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}


		}



        void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}
	}
}
