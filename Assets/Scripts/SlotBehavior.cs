using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBehavior : MonoBehaviour {

	Controller controller;
	RageSpline mySpline;
	public bool highlighted;
	public string targetName;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find("Controller").GetComponent<Controller>();
		mySpline = GetComponent<RageSpline>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver () {
		if (controller.dragging) {
			highlighted = true;
			mySpline.fillColor1 = new Color(1f,1f,1f,0.5f);
			mySpline.outlineColor1 = new Color(1f,1f,1f,1f);
			mySpline.RefreshMesh(true,true,true);
		}
	}

	void OnMouseExit () {
		highlighted = false;
		mySpline.fillColor1 = new Color(0.36f,0.73f,1f,0.0f);
		mySpline.outlineColor1 = new Color(0.36f,0.73f,1f,1.0f);
		mySpline.RefreshMesh(true,true,true);
	}

}
