using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CucumberThrower : MonoBehaviour , IPointerUpHandler, IPointerDownHandler {

	private bool canFire;

	private bool touched;
	private int pointerID;


	void Awake(){
		touched = false;
	}

	public bool CanFire(){
		return canFire;
	}

	public void OnPointerDown(PointerEventData data){
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			canFire = true;
		}
	}

	public void OnPointerUp(PointerEventData data){
		if (data.pointerId == pointerID) {
			canFire = false;
			touched = false;
		}
	}
}
