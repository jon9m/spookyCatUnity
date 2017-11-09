using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{

	public GameObject target;

	public Animator anim;

	public float stoppingDistance;

	public UnityEngine.AI.NavMeshAgent navMeshAgent;

	public bool finishedEating;

	public Coroutine waitFinishEatingCR;

	private Rigidbody navMeshAgentRigidBody;

	// Use this for initialization
	void Start ()
	{
		finishedEating = true;

		anim = gameObject.GetComponentInChildren<Animator> ();
		navMeshAgent = GetComponent <UnityEngine.AI.NavMeshAgent> ();

		navMeshAgent.stoppingDistance = stoppingDistance;	
		navMeshAgent.enabled = true;

		navMeshAgentRigidBody = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//navMeshAgent.SetDestination (target.transform.position);

		if ((!navMeshAgent.pathPending) && (navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete)) {
			if (navMeshAgent.hasPath) {
				if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) {														
					anim.SetInteger ("eatfinishraisehead", 1);
					anim.SetInteger ("walktofood", 1);

					anim.SetInteger ("eatpickup", 0);
					anim.SetInteger ("turnback", -1);
				} else {
					anim.SetInteger ("randomwalk", 0);

					anim.SetInteger ("walktofood", 0);
					anim.SetInteger ("eatpickup", 1);
					anim.SetInteger ("eatfinishraisehead", 0);

					navMeshAgent.enabled = false;

					finishedEating = false;
					waitFinishEatingCR = StartCoroutine (waitFinishEating (5f));
				}
			}
		}

		if (finishedEating == true) {
			anim.SetInteger ("eatfinishraisehead", 1);
			anim.SetInteger ("walktofood", 0);
			anim.SetInteger ("eatpickup", 0);


			int turn = Random.Range (0, 2);	
			if (anim.GetInteger ("turnback") == -1) { //To stop updating continously
				anim.SetInteger ("turnback", turn);
			}

			if (anim.GetInteger ("turnback") == 1) { //&& saw cucumber //TODO - also add the condition in animation
				anim.SetInteger ("lookupleft", 1);
				anim.SetInteger ("lookupright", 0);
			} else {
				anim.SetInteger ("lookupleft", 0);
				anim.SetInteger ("lookupright", 1);
			}
		}
		//Debug.Log ("turnback" + anim.GetInteger ("turnback"));
	}

	public IEnumerator waitFinishEating (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		finishedEating = true;
	}

	public void MoveCatRandomPosition (GameObject cat)
	{
		float walkRadius = 10;
		Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

		randomDirection += cat.transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition (randomDirection, out hit, walkRadius, 5);
		Vector3 finalPosition = hit.position;

		navMeshAgent.enabled = true;
		navMeshAgent.SetDestination (finalPosition);
	}

	void FixedUpdate ()
	{
		if ((anim != null) && (anim.GetInteger ("calmdown") != 1)) {
			navMeshAgentRigidBody.freezeRotation = false;
		} else {
			navMeshAgentRigidBody.freezeRotation = true;

			Quaternion oldRotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
			transform.rotation = oldRotation;
		}
	}
}
