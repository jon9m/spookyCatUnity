using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

	private CatMover catMover;

	Animator anim;
	float restartTimer;
	public float restartDelay = 5f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		if (catMover == null) {		
			catMover = FindObjectOfType<CatMover> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (catMover.gameOver == true) {
			anim.SetTrigger ("GameOver");

			restartTimer += Time.deltaTime;
			if (restartTimer > restartDelay) {
				//Application.LoadLevel (Application.loadedLevel);
				SceneManager.LoadScene(0);
			}
		}
	}
}
