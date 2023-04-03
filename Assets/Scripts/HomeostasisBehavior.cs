using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeostasisBehavior : MonoBehaviour {

	RageSpline bar;
	bool direction;
	float counter;

	public Color deepColor;
	public Color shallowColor;


	// Use this for initialization
	void Start () {
		bar = transform.Find("Bar").GetComponent<RageSpline>();
		counter = 0f;
	}

	// Update is called once per frame
	void Update () {
		if (counter < 1) {
			counter+=0.3f*Time.deltaTime;
			if (direction) {
				bar.fillColor1 = Color.Lerp(deepColor,shallowColor,Mathf.SmoothStep(0,1,counter));
			} else {
				bar.fillColor1 = Color.Lerp(shallowColor,deepColor,Mathf.SmoothStep(0,1,counter));
			}
			bar.RefreshMesh(true,true,true);
		} else {
			counter = 0;
			direction = !direction;
		}
	}


}
