using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionBehavior : MonoBehaviour {

	RageSpline mySpline;
	Color myColor;
	Color myOutline;

	Controller controller;
	public string myOption;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find("Controller").GetComponent<Controller>();
		mySpline = GetComponent<RageSpline>();
		myColor = mySpline.fillColor1;
		myOutline = mySpline.outlineColor1;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver () {
		mySpline.fillColor1 = new Color(myColor.r+0.2f,myColor.g+0.2f,myColor.b+0.2f,myColor.a);
		mySpline.outlineColor1 = new Color(myOutline.r+0.2f,myOutline.g+0.2f,myOutline.b+0.2f,myOutline.a);
		mySpline.RefreshMesh(true,true,true);
	}

	void OnMouseExit () {
		mySpline.fillColor1 = myColor;
		mySpline.outlineColor1 = myOutline;
		mySpline.RefreshMesh(true,true,true);
	}

	void OnMouseDown () {
		controller.chosenOption = myOption;
	}

}
