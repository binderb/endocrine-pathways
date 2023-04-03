using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxBehavior : MonoBehaviour {

	Text myText;
	float R;
	float G;
	float B;
	string cycling;

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text>();
		cycling = "toYellow";
	}

	// Update is called once per frame
	void Update () {
		Color myNewColor = new Color(0.8f,0.8f,0.8f,1f);
		float advanceFloat = 0.4f*Time.deltaTime;
		if (myText.enabled) {
			if (cycling == "toYellow") {
				R = 0.2f;
				G += advanceFloat;
				B = 0f;
				if (G >= 0.2f) cycling = "toGreen";
			} else if (cycling == "toGreen") {
				R -= advanceFloat;
				G = 0.2f;
				B = 0f;
				if (R <= 0f) cycling = "toCyan";
			} else if (cycling == "toCyan") {
				R = 0f;
				G = 0.2f;
				B += advanceFloat;
				if (B >= 0.2f) cycling = "toBlue";
			} else if (cycling == "toBlue") {
				R = 0f;
				G -= advanceFloat;
				B = 0.2f;
				if (G <= 0f) cycling = "toViolet";
			} else if (cycling == "toViolet") {
				R += advanceFloat;
				G = 0f;
				B = 0.2f;
				if (R >= 0.2f) cycling = "toRed";
			} else if (cycling == "toRed") {
				R = 0.2f;
				G = 0f;
				B -= advanceFloat;
				if (B <= 0f) cycling = "toYellow";
			}
			myText.color = myNewColor + new Color(R,G,B,1f);
		}
	}




}
