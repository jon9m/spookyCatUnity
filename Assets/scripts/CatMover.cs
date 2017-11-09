using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityStandardAssets.Characters.FirstPerson;

public class CatMover : MonoBehaviour
{

	public float catForce;
	public Rigidbody catRigidBody;
	public bool isCucumberBehind;
	public Slider damageSlider;

	public Vector3 randomPosition;

	public static int score;
	public Text scoreText;

	public Text debugText;

	public NavigationScript navigationScript;

	public bool gameOver;
	public bool gameCompleted;

	public Image sliderFill;

	public GameObject hitEffect;
	public GameObject deathEffect;

	void Start ()
	{
		catRigidBody = GetComponent<Rigidbody> ();
		isCucumberBehind = false;

		navigationScript = GetComponent<NavigationScript> ();	
		gameOver = false;
		gameCompleted = false;
	}

	void Awake ()
	{
		gameOver = false;
		gameCompleted = false;

		score = 0;
	}

	void FixedUpdate ()
	{
		if (catRigidBody.velocity.y < 0) {
			Vector3 catVelocity = catRigidBody.velocity;
			catVelocity.y = -5f;
			catRigidBody.velocity = catVelocity; 
		}
	}

	void Update ()
	{
		damageSlider.value = score;

		if (score != 0) {
			float greenNBlue = 1f - (score / 85f); 		//TODO

			sliderFill.color = new Color (sliderFill.color.r, greenNBlue, greenNBlue, sliderFill.color.a);
		}
	}

	void OnCollisionEnter (Collision col)
	{

		if (gameObject.CompareTag ("cat")) {
			if (col.gameObject.CompareTag ("fan")) {

				Rigidbody catDeadBody = col.gameObject.GetComponent<Rigidbody> ();
				if (catDeadBody != null) {
					catDeadBody.freezeRotation = false;

					Renderer renderer = gameObject.GetComponent<Renderer> ();
					if (renderer != null) {
						renderer.material.color = Color.red;
					}

					Renderer fanRenderer = col.gameObject.GetComponent<Renderer> ();
					if (fanRenderer != null) {
						fanRenderer.material.color = Color.red;
					}
				}					

				gameOver = true;

				Destroy (Instantiate (deathEffect, gameObject.transform.position, Quaternion.identity) as GameObject, 2f);
			}

			//Debug.Log ("Score " + score);

			if (score >= 85) {  			//TODO	
				if (!gameOver) {
					gameCompleted = true;
				}
			}

			if (col.gameObject.CompareTag ("static_to_dynamic")) {
				Rigidbody static_to_dynamic = col.gameObject.GetComponent<Rigidbody> ();
				if (static_to_dynamic != null) {
					static_to_dynamic.isKinematic = false;
				}
			}

			//If you hit the cat with the cucumber walk away
			if (col.gameObject.CompareTag ("cucumber")) {
				navigationScript.anim.SetInteger ("walkingrandom", 1);
			}

			if ((col.gameObject.CompareTag ("cucumber")) || ((col.gameObject.CompareTag ("catfood")))) {
				return;
			}


			//Debug.Log ("col.gameObject.tag "+ col.gameObject.tag);

			if (col.gameObject.CompareTag ("hittargets")) {
				CatHitsChecker checker = col.gameObject.GetComponent<CatHitsChecker> ();
				if (!checker.isCatHit) {
					score++;
					//scoreText.text = "SCORE : " + score;
					checker.isCatHit = true;

					StartCoroutine (checker.waitAndDestroy (10));
				}
			}

			if (gameOver != true) {
				if (col.gameObject.CompareTag ("hittargets")
				    || col.gameObject.CompareTag ("boundary")
				    || col.gameObject.CompareTag ("static")
				    || col.gameObject.CompareTag ("static_to_dynamic")) {
					Destroy (Instantiate (hitEffect, gameObject.transform.position, Quaternion.identity) as GameObject, 2f);
				}
			}
		}	

		//CALM THE CAT DOWN
		if (gameObject.CompareTag ("cat")) {
			if (col.gameObject.CompareTag ("floor")) {
				navigationScript.anim.SetInteger ("calmdown", 1);

				if (navigationScript.anim.GetInteger ("jumped") == 1) {
					navigationScript.anim.SetInteger ("jumped", 0);

					navigationScript.anim.SetInteger ("walkingrandom", 1);
				}
			}
		}
	}
}