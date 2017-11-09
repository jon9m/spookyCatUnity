using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.UI;

public class FPSCatHandler : MonoBehaviour
{

	public GameObject cookie;
	public GameObject cucumber;
	public GameObject cat;

	private GameObject cookieClone;
	public GameObject cucumberClone;

	public NavigationScript navigationScript;
	public CatMover catMover;


	public CharacterController fpsController;
//	private Rigidbody fpsRigidBody;

	public CucumberThrower cucumberThrower;
	public CookieThrower cookieThrower;
	public TouchPad touchPad;

	public Text debugText;

	AudioSource foodClip;
	public AudioClip foodAudioClip;  

	void placeCucumber ()
	{
		if (cucumberClone != null) {
			Destroy (cucumberClone, 0.0f);
		}
		cucumberClone = Instantiate (cucumber, transform.position + 1.5f * (transform.forward), transform.rotation);
		cucumberClone.GetComponent<Rigidbody> ().AddForce (transform.forward * 3, ForceMode.Impulse);
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (cat.transform.position - 2f * (cat.transform.forward) - 1f * (cat.transform.right), new Vector3 (2.5f, 2f, 4f));

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube (cat.transform.position - 2f * (cat.transform.forward) + 1f * (cat.transform.right), new Vector3 (2.5f, 2f, 4f));
	}

	void shootCatFood ()
	{
		if (cookieClone != null) {
			Destroy (cookieClone, 0.0f);
		}

		if (cucumberClone != null) {
			Destroy (cucumberClone, 0.0f);
		}
			
		//cookieClone = Instantiate (cookie, transform.position + 1.5f * (transform.forward), transform.rotation);
		//cookieClone.GetComponent<Rigidbody> ().AddForce (transform.forward * 3, ForceMode.Impulse);

		cookieClone = Instantiate (cookie, transform.position + 6f * (transform.forward), transform.rotation);
		//cookieClone.GetComponent<Rigidbody> ().AddForce (transform.forward * 3, ForceMode.Impulse);

		if ((navigationScript == null) && (cat != null)) {
			navigationScript = cat.GetComponent<NavigationScript> ();
		}

		if (navigationScript != null) {
			navigationScript.anim.SetInteger ("walkingrandom", 0);
			navigationScript.anim.SetInteger ("randomwalk", 0);

			navigationScript.anim.SetInteger ("calmdown", 1);
			navigationScript.anim.SetInteger ("eatpickup", 0);
			navigationScript.anim.SetInteger ("turnback", -1);

			navigationScript.anim.SetInteger ("eatfinishraisehead", 1);
			navigationScript.anim.SetInteger ("walktofood", 1);

			navigationScript.finishedEating = false;
			if (navigationScript.waitFinishEatingCR != null) {
				StopCoroutine (navigationScript.waitFinishEatingCR);
			}

			navigationScript.navMeshAgent.enabled = true;
			navigationScript.navMeshAgent.SetDestination (cookieClone.transform.position);
		}

		foodClip.clip = foodAudioClip;
		foodClip.Play ();
	}

	void setRandomDestination ()
	{
		navigationScript.navMeshAgent.SetDestination (catMover.randomPosition);
	}

	// Use this for initialization
	void Start ()
	{
		catMover = cat.GetComponent<CatMover> ();

		fpsController = GetComponent<CharacterController> ();

		foodClip = GetComponent<AudioSource> ();

		//if (fpsController) {
		//	fpsRigidBody = GetComponent<Rigidbody> ();
		//}
	}

	void FixedUpdate ()
	{	
		fpsMove ();

		//Camara move on cat
		if (navigationScript != null) {
			if (navigationScript.anim.GetInteger ("calmdown") == 0) {		
				fpsController.transform.LookAt (cat.transform);
			} else {
				fpsController.transform.eulerAngles= new Vector3(0f, fpsController.transform.eulerAngles.y, 0f);
				fpsController.transform.position = new Vector3 (fpsController.transform.position.x, 7.18f,fpsController.transform.position.z);
			}
		}
	}

	void fpsMove ()
	{
		Vector2 direction = touchPad.getDirection ();
		//Debug.Log ("direction " + direction);

		fpsController.Move (transform.forward * direction.y * 0.2f);
		fpsController.transform.Rotate (new Vector3 (0, direction.x, 0));
	}


	private float throwFoodTime;

	// Update is called once per frame
	void Update ()
	{		

		if (cucumberThrower.CanFire ()) {
			placeCucumber ();
		}
		if (cookieThrower.CanFire ()) {			
			if (throwFoodTime < 0) {
				throwFoodTime = 3f;
				shootCatFood ();
			}
		}

		throwFoodTime -= Time.deltaTime;


		//TODO - remove for mobile ? - disable fps controller script
		if (Input.GetKeyDown ("c")) {
			shootCatFood ();
		}

		if (Input.GetKeyDown ("v")) {
			placeCucumber ();
		}

			
		//Cucumber intersects the area
		if (cucumberClone != null) {
			//Bounds leftBound = new Bounds ((cat.transform.position - 1.5f * (cat.transform.forward) - 1f * (cat.transform.right)), new Vector3 (2f, 1f, 2f));
			//Bounds rightBound = new Bounds ((cat.transform.position - 1.5f * (cat.transform.forward) + 1f * (cat.transform.right)), new Vector3 (2f, 1f, 2f));

			Bounds leftBound = new Bounds (cat.transform.position - 2f * (cat.transform.forward) - 1f * (cat.transform.right), new Vector3 (2.5f, 2f, 4f));
			Bounds rightBound = new Bounds (cat.transform.position - 2f * (cat.transform.forward) + 1f * (cat.transform.right), new Vector3 (2.5f, 2f, 4f));

			Collider collider = cucumberClone.GetComponent<Collider> ();

			bool isInterscet = false;
			if (navigationScript != null) {
				if (navigationScript.anim.GetInteger ("turnback") == 1) { //Left
					isInterscet = leftBound.Intersects (collider.bounds);
				} else if (navigationScript.anim.GetInteger ("turnback") == 0) { //Right
					isInterscet = rightBound.Intersects (collider.bounds);
				}
			}
			catMover.isCucumberBehind = isInterscet;

			//Debug.Log ("Cucumber Intersect " + catMover.isCucumberBehind);
		}
	}
}
