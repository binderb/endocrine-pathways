using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunburstBehavior : MonoBehaviour {

	public int direction;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.back * direction * 30*Time.deltaTime);
	}
}
