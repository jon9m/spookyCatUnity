using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

	public Animator anim;
	public CharacterController controller;

	public float speed	= 6.0f;
	public float turnSpeed = 60.0f;
	//private Vector3 moveDirection = Vector3.zero;

	private Rigidbody rigidBody;
		

	void Awake(){
		Input.backButtonLeavesApp = true;
	}

	// Use this for initialization
	void Start ()
	{
		anim = gameObject.GetComponentInChildren<Animator> ();
		controller = GetComponent<CharacterController> ();

		rigidBody = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update ()
	{

		//if(Input.GetKeyDown(KeyCode.Escape) == true){
			//Application.Quit();
		//}
			

		/*
		if (Input.GetKey ("up")) {
			anim.SetInteger ("anim", 1);
		} else {
			anim.SetInteger ("anim", 0);
		}

		if (controller.isGrounded) {
			moveDirection = transform.forward * Input.GetAxis ("Vertical") * speed;
		}

		float turn = Input.GetAxis ("Horizontal");
		transform.Rotate (0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move (moveDirection * Time.deltaTime);
		moveDirection.y -=gravity * Time.deltaTime; 
		*/
	}

	void FixedUpdate ()
	{

		if (Input.GetKey ("up") || Input.GetKey ("down")) {
			anim.SetInteger ("walktofood", 1);
		} else {
			anim.SetInteger ("walktofood", 0);
		}


		if (rigidBody.position.y < 1.8) { // Finished falling down

			/*
			float moveHorizonatal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			moveDirection = transform.forward * Input.GetAxis ("Vertical") * speed;


			float turn = Input.GetAxis ("Horizontal");
			transform.Rotate (0, turn * turnSpeed * Time.deltaTime, 0);

			rigidBody.velocity = moveDirection * speed;
			*/
		}
	}
}
