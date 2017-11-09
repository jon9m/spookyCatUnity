using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour , IPointerUpHandler, IPointerDownHandler, IDragHandler {

	private Vector2 origin;
	private Vector2 direction;

	private Vector2 smoothDirection;
	public float smoothing;

	private bool touched;
	private int pointerID;


	void Awake(){
		touched = false;
		direction = Vector2.zero;
	}

	/*
	public Vector2 getDirection(){
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}
	*/

	public Vector2 getDirection(){
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;

		//return direction;
	}

	public void OnPointerDown(PointerEventData data){
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;

			origin = data.position;
		}
	}

	public void OnDrag(PointerEventData data){
		if (data.pointerId == pointerID) {
			Vector2 currentPosition = data.position;
			Vector2 directionRaw = currentPosition - origin;
			direction = directionRaw.normalized;

			//Debug.Log ("origin " + origin);
			//Debug.Log ("currentPosition " + currentPosition);
			//Debug.Log ("direction " + direction);
		}
	}

	public void OnPointerUp(PointerEventData data){
		if (data.pointerId == pointerID) {
			direction = Vector2.zero;
			touched = false;
		}
	}
}
