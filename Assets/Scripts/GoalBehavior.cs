using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalBehavior : MonoBehaviour {

	Controller controller;
	public Transform mySlot;
	public RageSpline mySlotSpline;

	public string myTitle;
	public string myText;
	public string mySolution;

	public bool hoveringWithSolution = false;
	public bool satisfied = false;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find("Controller").GetComponent<Controller>();
		mySlotSpline = mySlot.GetComponent<RageSpline>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseEnter () {
		if (!controller.locked && !controller.nodrag && !controller.dragging) {
			Transform goalBox = GameObject.Find("goalBox").transform;
			GameObject.Find("goalBoxTitle").GetComponent<Text>().text = myTitle;
			GameObject.Find("goalBoxText").GetComponent<Text>().text = myText;
			if (satisfied) GameObject.Find("goalBoxAchieved").GetComponent<Text>().text = "Goal Achieved!";
			else GameObject.Find("goalBoxAchieved").GetComponent<Text>().text = "";
			goalBox.position = transform.position + Vector3.right*8f + Vector3.down*90f;
		} else if (!controller.locked && !controller.nodrag && controller.dragging && !satisfied) {
			if (controller.selectedCard.GetComponent<CardBehavior>().myTitle == mySolution) {
				mySlotSpline.outlineColor1 = Color.white;
				mySlotSpline.RefreshMesh(true,true,true);
				hoveringWithSolution = true;
				StartCoroutine (hoveringShimmer());
			}
		}
	}

	void OnMouseExit () {
		Transform goalBox = GameObject.Find("goalBox").transform;
		goalBox.position = goalBox.position + Vector3.forward*100f;
		if (satisfied) {
			mySlotSpline.outlineColor1 = new Color(0f,0.9f,0f);
		} else {
			mySlotSpline.outlineColor1 = new Color(0.36f,0.73f,1f,1.0f);
		}
		mySlotSpline.RefreshMesh(true,true,true);
		hoveringWithSolution = false;
	}

	IEnumerator hoveringShimmer () {
		while (hoveringWithSolution) {
			StartCoroutine(flashSlot());
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator flashSlot () {
		float t = 0;
		Transform slotFlash = (Transform)Instantiate(mySlot);
		slotFlash.position = new Vector3(mySlot.transform.position.x,mySlot.transform.position.y,-300f);
		slotFlash.GetComponent<RageSpline>().outlineColor1 = Color.white;
		slotFlash.GetComponent<RageSpline>().fillColor1 = Color.clear;
		slotFlash.GetComponent<RageSpline>().RefreshMesh(true,true,true);
		Vector3 initSlotScale = slotFlash.localScale;
		while (t < 1) {
			t+=1f*Time.deltaTime;
			slotFlash.localScale = Vector3.Lerp(initSlotScale,initSlotScale*3f,Mathf.SmoothStep(0,1,t));
			slotFlash.GetComponent<RageSpline>().outlineColor1 = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			slotFlash.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		Destroy(slotFlash.gameObject);
	}

}
