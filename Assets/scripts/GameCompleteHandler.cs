using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteHandler : MonoBehaviour {

	private CatMover catMover;

	Animator anim;
	float restartTimer;
	public float restartDelay = 7f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		if (catMover == null) {		
			catMover = FindObjectOfType<CatMover> ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (catMover.gameCompleted == true) {
			anim.SetTrigger ("GameCompleted");

			restartTimer += Time.deltaTime;
			if (restartTimer > restartDelay) {
				SceneManager.LoadScene(0);
			}
		}
	}
}
