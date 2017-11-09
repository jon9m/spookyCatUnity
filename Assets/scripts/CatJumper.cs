using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatJumper : MonoBehaviour
{

	public float catForce;
	private Rigidbody catRigidBody;
	private GameObject cat;

	private CatMover catMover;

	private float timer;
	public float wanderTimer;

	AudioSource jumpClip;

	void catJump ()
	{
		if (catMover.isCucumberBehind == true) {
			catRigidBody.AddForce (transform.up * catForce * 0.7f, ForceMode.Impulse);
			catRigidBody.AddForce (transform.forward * -catForce * 0.5f, ForceMode.Impulse);

			catMover.navigationScript.anim.SetInteger ("sawcucumber", 1);
			catMover.navigationScript.anim.SetInteger ("calmdown", 0);

			catMover.navigationScript.anim.SetInteger ("walkingrandom", 0);

			//Jumped 
			catMover.navigationScript.anim.SetInteger ("jumped", 1);

			jumpClip.Play ();
		} else {
			catMover.navigationScript.anim.SetInteger ("sawcucumber", 0);
			catMover.navigationScript.anim.SetInteger ("calmdown", 1);

			catMover.navigationScript.anim.SetInteger ("walkingrandom", 1);
		}
	}

	void Start ()
	{
		timer = wanderTimer;

		cat = transform.parent.gameObject;
		catRigidBody = transform.parent.GetComponent<Rigidbody> ();

		jumpClip = GetComponent<AudioSource> ();


		if ((catMover == null) && (cat != null)) {
			catMover = cat.GetComponent<CatMover> ();
		}
	}

	void Update ()
	{
		if (catMover.navigationScript.anim.GetInteger ("walkingrandom") == 1) {
			timer += Time.deltaTime;
			if (timer > wanderTimer) {
				timer = 0;
				if (catMover.navigationScript.anim.GetInteger ("randomwalk") != 0) {
					catMover.navigationScript.anim.SetInteger ("randomwalk", 0);
				}
			} else {
				if (catMover.navigationScript.anim.GetInteger ("randomwalk") != 1) {					
					catMover.navigationScript.anim.SetInteger ("randomwalk", 1);

					catMover.navigationScript.MoveCatRandomPosition (cat);
				}
			}
		}
	}
}