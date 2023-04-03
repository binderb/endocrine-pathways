using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour {

	Vector3 myScale;
	Controller controller;

	// Use this for initialization
	void Start () {
		myScale = transform.localScale;
		controller = GameObject.Find("Controller").GetComponent<Controller>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
		if ((transform.parent.parent != null && (transform.parent.parent.name == "MessageBox" || transform.parent.parent.name == "InstructionsBox")) || (!controller.locked && !controller.nodrag)) StartCoroutine(popButton());
	}

	IEnumerator popButton() {
		Vector3 initScale = myScale;
		float t = 0;
		while (t < 1) {
			t += 8*Time.deltaTime;
			transform.localScale = Vector3.Lerp(1.1f*initScale,initScale,Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		t = 0;
		/*while (t < 1) {
			t += 8*Time.deltaTime;
			transform.localScale = Vector3.Lerp(1.05f*initScale,initScale,Mathf.SmoothStep(0,1,t));
			yield return 0;
		}*/
	}

}
