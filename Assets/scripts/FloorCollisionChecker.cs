using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollisionChecker : MonoBehaviour {

	public GameObject hitEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag ("hittargets")
		    || col.gameObject.CompareTag ("static")
		    || col.gameObject.CompareTag ("static_to_dynamic")) {

			Destroy (Instantiate (hitEffect, col.transform.position, Quaternion.identity) as GameObject, 2f);
		}
	}
}
