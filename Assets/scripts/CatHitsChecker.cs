using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHitsChecker : MonoBehaviour {

	public bool isCatHit;
	CatMover catMover;

	bool isDead;
	float fadeSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		isCatHit = false;
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead) {
			Renderer renderer = gameObject.GetComponent<Renderer> ();
			if((renderer != null) && (renderer.material.HasProperty("_Color"))){
				renderer.material.color = Color.Lerp (renderer.material.color, new Color(0f, 0f, 0f, 0f), fadeSpeed * Time.deltaTime); //TODO - color
			}
		}
	}

	public IEnumerator waitAndSink (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);

		Collider[] colliders = gameObject.GetComponents<Collider> ();
		foreach (Collider coll in colliders) {
			coll.isTrigger = true;	
		}
	}

	public IEnumerator waitAndDestroy (float waitTime)
	{

		StartCoroutine(waitAndSink (waitTime/2));

		isDead = true;
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}

	void OnCollisionEnter (Collision col)
	{

		if (catMover == null) {		
			catMover = FindObjectOfType<CatMover> ();
		}

		if (col.gameObject.CompareTag ("floor")) {
			if (!isCatHit) {
				CatMover.score++;
				//catMover.scoreText.text = "SCORE : " + CatMover.score;
				isCatHit = true;

				StartCoroutine(waitAndDestroy (10));
			}
		}	
	}
}
