using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardBehavior : MonoBehaviour {

	Controller controller;
	public Vector3 handPos;
	public Quaternion handRot;
	public Vector3 handScale;

	public string myTitle;
	public string myText;
	public string myTargets;
	public string myType;
	public int handIndex;
	public int myDictionaryIndex;
	public string chosenTargetSlot;

	private int myFullTitleSize;
	private int myFullTextSize;
	private Vector2 myFullTitleBoxSize;
	private Vector2 myFullTextBoxSize;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find("Controller").GetComponent<Controller>();
		handPos = transform.position;
		handRot = transform.rotation;
		handScale = transform.localScale;
		//myFullTitleSize = transform.Find("card_canvas").Find("card_title").GetComponent<Text>().fontSize;
		//myFullTextSize = transform.Find("card_canvas").Find("card_text").GetComponent<Text>().fontSize;
		//myFullTitleBoxSize = transform.Find("card_canvas").Find("card_title").GetComponent<RectTransform>().sizeDelta;
		//myFullTextBoxSize = transform.Find("card_canvas").Find("card_text").GetComponent<RectTransform>().sizeDelta;
	}

	// Update is called once per frame
	void Update () {

	}

	// --------------------------
	// UI Functions
	// --------------------------

	void OnMouseDown () {
		if (!controller.locked && !controller.nodrag && !controller.zoomed && !controller.dragging && !controller.zoomed && tag != "reward_card") {
			controller.dragging = true;
			controller.selectedCard = transform;
			controller.dragInit = transform.position;
			controller.mouseInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			gameObject.layer = 2;
			StartCoroutine(changeOpacity(0.3f));
			StartCoroutine(changeRot(Quaternion.identity));
		} else if (!controller.locked && !controller.zoomed && !controller.dragging && !controller.zoomed && tag == "reward_card") {
			controller.chosenReward = myDictionaryIndex;
		}
	}

	void OnMouseOver () {
		if (!controller.locked && !controller.zoomed && Input.GetMouseButtonDown(1) && tag != "reward_card") {
			transform.position = new Vector3(480f,300f,-495f);
			transform.rotation = Quaternion.Euler(0f,0f,0f);
			transform.localScale = new Vector3(1f,1f,1f);
			controller.selectedCard = transform;
			controller.zoomed = true;
			controller.locked = true;
		} else if (!controller.locked && !controller.zoomed && Input.GetMouseButtonDown(1) && tag == "reward_card") {
			Transform copy = (Transform)Instantiate(transform);
			copy.position = new Vector3(480f,300f,-495f);
			copy.rotation = Quaternion.Euler(0f,0f,0f);
			copy.localScale = new Vector3(1f,1f,1f);
			controller.selectedCard = copy.transform;
			controller.zoomed = true;
			controller.locked = true;
		}
	}

	void OnMouseUp () {
		if (controller.dragging) {
			GameObject theSlot = null;
			GameObject theGoal = null;
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("cardslot")) {
				if (g.GetComponent<SlotBehavior>().highlighted) {theSlot = g;}
			}
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("goalcard")) {
				if (g.GetComponent<GoalBehavior>().hoveringWithSolution) {theGoal = g;}
			}
			// If card is hovering over a slot
			if (theSlot != null) {
				controller.dragging = false;
				string[] myTargetArray = myTargets.Split(',');
				if (Array.Exists(myTargetArray, element => element == theSlot.GetComponent<SlotBehavior>().targetName)) {
					chosenTargetSlot = theSlot.GetComponent<SlotBehavior>().targetName;
					StartCoroutine(receiveCorrectTarget(theSlot));
				} else {
					StartCoroutine(receiveIncorrectTarget(theSlot));
				}
			} else if (theGoal != null) {
				// If card is hovering over the correct goal
				controller.dragging = false;
				StartCoroutine(receiveGoal(theGoal));

			} else {
				// If card is not hovering over a slot
				controller.dragging = false;
				gameObject.layer = 0;
				StartCoroutine(move(controller.dragInit));
				StartCoroutine(changeOpacity(1.0f));
				StartCoroutine(changeRot(handRot));
			}
		}
	}

	// --------------------------
	// Gameplay Functions
	// --------------------------

	IEnumerator receiveCorrectTarget (GameObject theSlot) {
		controller.locked = true;
		Vector3 initScale = transform.localScale;
		Vector3 initPos = transform.position;
		Quaternion initRot = transform.rotation;
		float initOpacity = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color.a;
		Color fill = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color;

		// Dock card with slot
		float t = 0;
		while (t < 1) {
			t+=3*Time.deltaTime;
			transform.localScale = Vector3.Lerp(initScale,theSlot.transform.localScale,Mathf.SmoothStep(0,1,t));
			transform.position = Vector3.Lerp(initPos,theSlot.transform.position,Mathf.SmoothStep(0,1,t));
			transform.rotation = Quaternion.Lerp(initRot,theSlot.transform.rotation,Mathf.SmoothStep(0,1,t));
			transform.Find("card_sprite").GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(fill.r,fill.g,fill.b,initOpacity),new Color(fill.r,fill.g,fill.b,1f),Mathf.SmoothStep(0,1,t));
			//GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		transform.position = transform.position + Vector3.forward*1000;
		gameObject.tag = "exempt_card";
		controller.HAND.RemoveAt(handIndex);
		controller.displayHand();

		// Add flare
		t = 0;
		Transform slotFlash = (Transform)Instantiate(theSlot.transform);
		slotFlash.position = new Vector3(theSlot.transform.position.x,theSlot.transform.position.y,-95f);
		slotFlash.GetComponent<RageSpline>().outlineColor1 = Color.white;
		slotFlash.GetComponent<RageSpline>().fillColor1 = Color.clear;
		slotFlash.GetComponent<RageSpline>().RefreshMesh(true,true,true);
		Vector3 initSlotScale = slotFlash.localScale;
		while (t < 1) {
			t+=3*Time.deltaTime;
			slotFlash.localScale = Vector3.Lerp(initSlotScale,initSlotScale*2f,Mathf.SmoothStep(0,1,t));
			slotFlash.GetComponent<RageSpline>().outlineColor1 = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			slotFlash.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		Destroy(slotFlash.gameObject);

		// Highlight target organ(s)
		t = 0;
		string organLocation = "";
		if (controller.currentGender == "Female" && controller.currentSystem == "Endocrine") organLocation = "female_endocrine_organs";
		if (controller.currentGender == "Female" && controller.currentSystem == "Other") organLocation = "female_somatic_organs";
		if (controller.currentGender == "Male" && controller.currentSystem == "Endocrine") organLocation = "male_endocrine_organs";
		if (controller.currentGender == "Male" && controller.currentSystem == "Other") organLocation = "male_endocrine_organs";
		List<Transform> organSprites = new List<Transform>();
		foreach (Transform g in GameObject.Find(organLocation).transform) {
			if (g.name == theSlot.GetComponent<SlotBehavior>().targetName) organSprites.Add(g);

		}
		List<Vector3> initOrganScales = new List<Vector3>();
		List<Vector3> initOrganPos = new List<Vector3>();
		foreach (Transform g in organSprites) {
			initOrganScales.Add(g.localScale);
			initOrganPos.Add(g.position);
			g.position = new Vector3(g.position.x,g.position.y,-200f);
			Transform sunburst1 = (Transform)Instantiate(controller.sunburst);
			Transform sunburst2 = (Transform)Instantiate(controller.sunburst);
			sunburst1.localScale = Vector3.zero;
			sunburst2.localScale = Vector3.zero;
			sunburst1.position = new Vector3(g.position.x,g.position.y,-180);
			sunburst2.position = new Vector3(g.position.x,g.position.y,-181);
			sunburst2.GetComponent<SunburstBehavior>().direction = -1;
		}
		while (t < 1) {
			t+=3*Time.deltaTime;
			for (int i=0;i<organSprites.Count;i++) {
				//organSprites[i].localScale = Vector3.Lerp(initOrganScales[i],initOrganScales[i]*2f,Mathf.SmoothStep(0,1,t));
			}
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("sunburst")) {
				g.transform.localScale = Vector3.Lerp(Vector3.zero,Vector3.one,Mathf.SmoothStep(0,1,t));
			}
			yield return 0;
		}

		// Determine what the target organ should do in response
		string clickText = "(left-click to continue)";
		switch(myTitle) {

			case "GnRH":
				controller.banner_title.text = "";
				controller.banner_subtitle.text = "";
				controller.banner_clickprompt.text = "";
				controller.banner_optionstitle.text = "Which hormone do you want to obtain?";
				yield return StartCoroutine(showOptions("•   LH,•   FSH","LH,FSH"));
				yield return StartCoroutine(teardropRoutine());
				if (controller.chosenOption == "LH") controller.provideCard(4);
				else if (controller.chosenOption == "FSH") controller.provideCard(5);
				break;

			case "TRH":
				controller.banner_title.text = "Obtained: TSH!";
				controller.banner_subtitle.text = "Hormone secreted from the anterior pituitary gland.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(6); // TSH
				break;

			case "GHRH":
				controller.banner_title.text = "Obtained: GH!";
				controller.banner_subtitle.text = "Hormone secreted from the anterior pituitary gland.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(7); // GH
				break;

			case "CRH":
				controller.banner_title.text = "Obtained: ACTH!";
				controller.banner_subtitle.text = "Hormone secreted from the anterior pituitary gland.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(8); // ACTH
				break;

			case "TSH":
				controller.banner_title.text = "Obtained: TH!";
				controller.banner_subtitle.text = "Hormone secreted from the thyroid gland.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(9); // TH
				break;

			case "ACTH":
				controller.banner_title.text = "";
				controller.banner_subtitle.text = "";
				controller.banner_clickprompt.text = "";
				controller.banner_optionstitle.text = "Which hormone do you want to obtain?";
			  yield return StartCoroutine(showOptions("•   Aldosterone,•   Cortisol","Aldosterone,Cortisol"));
				yield return StartCoroutine(teardropRoutine());
				if (controller.chosenOption == "Cortisol") controller.provideCard(12);
				else if (controller.chosenOption == "Aldosterone") controller.provideCard(13);
				break;

			case "GH":
				if (chosenTargetSlot == "liver") {
					controller.banner_title.text = "Effect: Elevated Blood Glucose!";
					controller.banner_subtitle.text = "Stored glycogen is digested into glucose and released.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(19); // Glucose Up
				} else if (chosenTargetSlot == "adipose") {
					controller.banner_title.text = "Effect: Release of Free Fatty Acids!";
					controller.banner_subtitle.text = "Adipocytes release free fatty acids into the bloodstream.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(24); // FAs Up
				} else if (chosenTargetSlot == "muscle") {
					controller.banner_title.text = "Effect: Elevated Blood Glucose!";
					controller.banner_subtitle.text = "Stored glycogen is digested into glucose and released.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(19); // Glucose Up
				} else if (chosenTargetSlot == "bone") {
					controller.banner_title.text = "Effect: Decreased Blood Calcium!";
					controller.banner_subtitle.text = "GH has stimulated osteoblast activity, depleting blood calcium.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(27); // Calcium Down
				}
				break;

			case "IGF1":
				if (chosenTargetSlot == "liver") {
					controller.banner_title.text = "Effect: Elevated Blood Glucose!";
					controller.banner_subtitle.text = "Stored glycogen is digested into glucose and released.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(19); // Glucose Up
				} else if (chosenTargetSlot == "adipose") {
					controller.banner_title.text = "Effect: Release of Free Fatty Acids!";
					controller.banner_subtitle.text = "Adipocytes release free fatty acids into the bloodstream.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(24); // FAs Up
				} else if (chosenTargetSlot == "muscle") {
					controller.banner_title.text = "Effect: Elevated Blood Glucose!";
					controller.banner_subtitle.text = "Stored glycogen is digested into glucose and released.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(19); // Glucose Up
				} else if (chosenTargetSlot == "bone") {
					controller.banner_title.text = "Effect: Decreased Blood Calcium!";
					controller.banner_subtitle.text = "GH has stimulated osteoblast activity, depleting blood calcium.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(27); // Calcium Down
				}
				break;

			case "LH":
				if (chosenTargetSlot == "ovaries") {
					controller.banner_title.text = "Effect: Ovulation!";
					controller.banner_subtitle.text = "Luteinizing hormone stimulates ovulation in females.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(21); // Ovulation
				} else if (chosenTargetSlot == "testes") {
					controller.banner_title.text = "Obtained: Testosterone!";
					controller.banner_subtitle.text = "Hormone secreted from the testes.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(18); // Testosterone
				}
				break;

			case "FSH":
				if (chosenTargetSlot == "ovaries") {
					controller.banner_title.text = "Obtained: Estradiol!";
					controller.banner_subtitle.text = "Hormone secreted from the ovaries.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(17); // Estradiol
				} else if (chosenTargetSlot == "testes") {
					controller.banner_title.text = "Effect: Spermatogenesis!";
					controller.banner_subtitle.text = "FSH stimulates sperm production in males.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(22); // Spermatogenesis
				}
				break;

			case "Testosterone":
				controller.banner_title.text = "Effect: Spermatogenesis!";
				controller.banner_subtitle.text = "Testosterone stimulates sperm production in males.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(22); // Spermatogenesis
				break;

			case "TH":
				controller.banner_title.text = "";
				controller.banner_subtitle.text = "";
				controller.banner_clickprompt.text = "";
				controller.banner_optionstitle.text = "Which effect do you want to utilize?";
				yield return StartCoroutine(showOptions("•   Elevated Body Temperature,•   Decreased Blood Glucose","Elevated Body Temperature,Decreased Blood Glucose"));
				yield return StartCoroutine(teardropRoutine());
				if (controller.chosenOption == "Elevated Body Temperature") controller.provideCard(23);
				else if (controller.chosenOption == "Decreased Blood Glucose") controller.provideCard(20);
				break;

			case "Elevated Blood Glucose":
				controller.banner_title.text = "Obtained: Insulin!";
				controller.banner_subtitle.text = "Hormone secreted from the pancreas.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(15); // Insulin
				break;

			case "Decreased Blood Glucose":
				controller.banner_title.text = "Obtained: Glucagon!";
				controller.banner_subtitle.text = "Hormone secreted from the pancreas.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(14); // Glucagon
				break;

			case "Elevated Blood Calcium":
				controller.banner_title.text = "Obtained: Calcitonin!";
				controller.banner_subtitle.text = "Hormone secreted from the thyroid.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(10); // Calcitonin
				break;

			case "Decreased Blood Calcium":
				controller.banner_title.text = "Obtained: PTH!";
				controller.banner_subtitle.text = "Hormone secreted from the parathyroid glands.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(11); // PTH
				break;

			case "Glucagon":
				if (chosenTargetSlot == "liver" || chosenTargetSlot == "muscle") {
					controller.banner_title.text = "Effect: Elevated Blood Glucose!";
					controller.banner_subtitle.text = "Stored glycogen is digested into glucose and released.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(19); // Glucose Up
				} else if (chosenTargetSlot == "adipose") {
					controller.banner_title.text = "Effect: Release of Free Fatty Acids!";
					controller.banner_subtitle.text = "Adipocytes release free fatty acids into the bloodstream.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(24); // FAs Up
				}
				break;

			case "Insulin":
				controller.banner_title.text = "Effect: Decreased Blood Glucose!";
				controller.banner_subtitle.text = "Glucose is absorbed by cells, removing it from the bloodstream.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(20); // Glucose down
				break;

			case "Cortisol":
				if (chosenTargetSlot == "liver") {
					controller.banner_title.text = "Effect: Elevated Blood Glucose!";
					controller.banner_subtitle.text = "Stored glycogen is digested into glucose and released.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(19); // Glucose Up
				} else if (chosenTargetSlot == "adipose") {
					controller.banner_title.text = "Effect: Release of Free Fatty Acids!";
					controller.banner_subtitle.text = "Adipocytes release free fatty acids into the bloodstream.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(24); // FAs Up
				} else if (chosenTargetSlot == "muscle") {
					controller.banner_title.text = "Effect: Elevated Blood Glucose!";
					controller.banner_subtitle.text = "Stored glycogen is digested into glucose and released.";
					controller.banner_clickprompt.text = clickText;
					controller.banner_optionstitle.text = "";
					yield return StartCoroutine(showBanner());
					yield return StartCoroutine(teardropRoutine());
					controller.provideCard(19); // Glucose Up
				}
				break;

			case "Aldosterone":
				controller.banner_title.text = "Effect: Increased Water Retention!";
				controller.banner_subtitle.text = "Kidney function is modulated to retain more water.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(25); // Water Up
				break;

			case "PTH":
				controller.banner_title.text = "Effect: Elevated Blood Calcium!";
				controller.banner_subtitle.text = "Blood calcium levels have increased.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(26); // Calcium Up
				break;

			case "Calcitonin":
				controller.banner_title.text = "Effect: Decreased Blood Calcium!";
				controller.banner_subtitle.text = "Blood calcium levels have decreased.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(27); // Calcium Down
				break;

			case "OT":
				controller.banner_title.text = "Effect: Uterine Contractions!";
				controller.banner_subtitle.text = "Oxytocin has stimulated the smooth muscle of the uterus.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(30); // Uterine contractions
				break;

			case "ADH":
				controller.banner_title.text = "Effect: Increased Water Retention!";
				controller.banner_subtitle.text = "Kidney function is modulated to retain more water.";
				controller.banner_clickprompt.text = clickText;
				controller.banner_optionstitle.text = "";
				yield return StartCoroutine(showBanner());
				yield return StartCoroutine(teardropRoutine());
				controller.provideCard(25); // Water Up
				break;

			case "Neuroendocrine Stimulation":
				controller.banner_title.text = "";
				controller.banner_subtitle.text = "";
				controller.banner_clickprompt.text = "";
				controller.banner_optionstitle.text = "Which part of the pituitary do you want to stimulate?";
				yield return StartCoroutine(showOptions("•   Anterior Pituitary,•   Posterior Pituitary","Anterior,Posterior"));
				if (controller.chosenOption == "Anterior") {
					controller.banner_optionstitle.text = "Which hormone do you want to obtain?";
					yield return StartCoroutine(showOptions("•   GHRH,•   TRH,•   CRH,•   GnRH","GHRH,TRH,CRH,GnRH"));
					yield return StartCoroutine(teardropRoutine());
					if (controller.chosenOption == "GHRH") controller.provideCard(2);
					else if (controller.chosenOption == "TRH") controller.provideCard(1);
					else if (controller.chosenOption == "CRH") controller.provideCard(3);
					else if (controller.chosenOption == "GnRH") controller.provideCard(0);
				} else if (controller.chosenOption == "Posterior") {
					controller.banner_optionstitle.text = "Which hormone do you want to obtain?";
					yield return StartCoroutine(showOptions("•   OT,•   ADH","OT,ADH"));
					yield return StartCoroutine(teardropRoutine());
					if (controller.chosenOption == "OT") controller.provideCard(28);
					else if (controller.chosenOption == "ADH") controller.provideCard(29);
				}

				break;


			default:
				// Do nothing
				break;
		}

		// Clean up
		t = 0;
		while (t < 1) {
			t+=3*Time.deltaTime;
			for (int i=0;i<organSprites.Count;i++) {
				//organSprites[i].localScale = Vector3.Lerp(initOrganScales[i]*2f,initOrganScales[i],Mathf.SmoothStep(0,1,t));
			}
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("sunburst")) {
				g.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			}
			yield return 0;
		}
		for (int i=0;i<organSprites.Count;i++) {organSprites[i].position = initOrganPos[i];}
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("sunburst")) Destroy(g);

		// Destroy this card, now that it's done influencing game events
		controller.locked = false;
		Destroy(gameObject);

	}

	IEnumerator receiveIncorrectTarget (GameObject theSlot) {
		controller.locked = true;
		Vector3 initScale = transform.localScale;
		Vector3 initPos = transform.position;
		Quaternion initRot = transform.rotation;
		float initOpacity = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color.a;
		Color fill = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color;

		// Dock card with slot
		float t = 0;
		while (t < 1) {
			t+=3*Time.deltaTime;
			transform.localScale = Vector3.Lerp(initScale,theSlot.transform.localScale,Mathf.SmoothStep(0,1,t));
			transform.position = Vector3.Lerp(initPos,theSlot.transform.position,Mathf.SmoothStep(0,1,t));
			transform.rotation = Quaternion.Lerp(initRot,theSlot.transform.rotation,Mathf.SmoothStep(0,1,t));
			transform.Find("card_sprite").GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(fill.r,fill.g,fill.b,initOpacity),new Color(fill.r,fill.g,fill.b,1f),Mathf.SmoothStep(0,1,t));
			//GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		transform.position = transform.position + Vector3.forward*1000;
		gameObject.tag = "exempt_card";
		controller.HAND.RemoveAt(handIndex);
		controller.displayHand();

		// Add flare
		t = 0;
		Transform slotFlash = (Transform)Instantiate(theSlot.transform);
		slotFlash.position = new Vector3(theSlot.transform.position.x,theSlot.transform.position.y,-95f);
		slotFlash.GetComponent<RageSpline>().outlineColor1 = Color.red;
		slotFlash.GetComponent<RageSpline>().fillColor1 = Color.clear;
		theSlot.GetComponent<RageSpline>().fillColor1 = Color.red;
		slotFlash.GetComponent<RageSpline>().RefreshMesh(true,true,true);
		theSlot.GetComponent<RageSpline>().RefreshMesh(true,true,true);
		Vector3 initSlotScale = slotFlash.localScale;
		while (t < 1) {
			t+=3*Time.deltaTime;
			slotFlash.localScale = Vector3.Lerp(initSlotScale,initSlotScale*2f,Mathf.SmoothStep(0,1,t));
			slotFlash.GetComponent<RageSpline>().outlineColor1 = Color.Lerp(Color.red,new Color(1f,0f,0f,0f),Mathf.SmoothStep(0,1,t));
			theSlot.GetComponent<RageSpline>().fillColor1 = Color.Lerp(Color.red,new Color(1f,0f,0f,0f),Mathf.SmoothStep(0,1,t));
			slotFlash.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			theSlot.GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		Destroy(slotFlash.gameObject);
		controller.showIncorrectMessage(myDictionaryIndex);

		// Don't destroy this card, because we still need its information if the user asks for an explanation.
		// The dismissal command will take care of this when the user closes the message box.
		//Destroy(gameObject);
	}

	IEnumerator receiveGoal (GameObject theGoal) {
		controller.locked = true;
		controller.goalsSatisfied++;
		Vector3 initScale = transform.localScale;
		Vector3 initPos = transform.position;
		Quaternion initRot = transform.rotation;
		float initOpacity = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color.a;
		Color fill = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color;

		// Dock card with slot
		float t = 0;
		RageSpline goalSlot = theGoal.GetComponent<GoalBehavior>().mySlotSpline;
		while (t < 1) {
			t+=3*Time.deltaTime;
			transform.localScale = Vector3.Lerp(initScale,goalSlot.transform.localScale,Mathf.SmoothStep(0,1,t));
			transform.position = Vector3.Lerp(initPos,goalSlot.transform.position,Mathf.SmoothStep(0,1,t));
			transform.rotation = Quaternion.Lerp(initRot,goalSlot.transform.rotation,Mathf.SmoothStep(0,1,t));
			transform.Find("card_sprite").GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(fill.r,fill.g,fill.b,initOpacity),new Color(fill.r,fill.g,fill.b,1f),Mathf.SmoothStep(0,1,t));
			//GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
		transform.position = transform.position + Vector3.forward*1000;
		gameObject.tag = "exempt_card";
		controller.HAND.RemoveAt(handIndex);
		controller.displayHand();
		if (controller.goalsSatisfied == 3) controller.hideHand();

		// Add flare
		GoalBehavior goalBehavior = theGoal.GetComponent<GoalBehavior>();
		goalBehavior.satisfied = true;
		goalBehavior.hoveringWithSolution = false;
		goalBehavior.mySlotSpline.outlineColor1 = new Color(0f,0.9f,0f);
		goalBehavior.mySlotSpline.RefreshMesh(true,true,true);
		t = 0;
		while (t < 1) {
			t+=1f*Time.deltaTime;
			goalSlot.fillColor1 = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			goalSlot.RefreshMesh(true,true,true);
			yield return 0;
		}

		if (controller.goalsSatisfied < 3) {
			// Present new card options for player
			controller.banner_title.text = "";
			controller.banner_subtitle.text = "";
			controller.banner_clickprompt.text = "\nLeft-Click: Select     Right-Click: Inspect";
			controller.banner_optionstitle.text = "Choose a new card to add to your hand:";
			yield return StartCoroutine(showRewards());
		} else {
			// If all goals have been satisfied, start a new round
			controller.completeRound();
		}

		// Destroy this card, now that it's done influencing game events
		controller.locked = false;
		Destroy(gameObject);
	}

	IEnumerator teardropRoutine () {
		float t = 0;
		Transform teardrop = GameObject.Find("teardrop").transform;
		Vector3 initPos = teardrop.position;
		while (t < 1) {
			t+=1f*Time.deltaTime;
			teardrop.position = Vector3.Lerp(initPos,new Vector3(initPos.x,50,initPos.z),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		teardrop.position = initPos;
	}

	IEnumerator showBanner () {
		float t = 0;
		Transform banner = GameObject.Find("banner").transform;
		while (t < 1) {
			t+=4f*Time.deltaTime;
			banner.position = Vector3.Lerp(new Vector3(-420f,316,-400),new Vector3(220f,316,-400),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		controller.locked = false;
		controller.nodrag = true;
		while (!Input.GetMouseButtonDown(0)) {
			yield return 0;
			// Hopefully won't crash everything
		}
		controller.locked = true;
		controller.nodrag = false;
		t = 0;
		while (t < 1) {
			t+=4f*Time.deltaTime;
			banner.position = Vector3.Lerp(new Vector3(220f,316,-400),new Vector3(-420f,316,-400),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
	}

	IEnumerator showOptions (string textString, string optionString) {
		Transform banner = GameObject.Find("banner").transform;
		string[] optionTexts = textString.Split(',');
		string[] options = optionString.Split(',');
		for (int i=0;i<optionTexts.Length;i++) {
			Transform option_i = (Transform)Instantiate(controller.option);
			option_i.position = new Vector3 (-386,365-45*i,-410);
			option_i.SetParent(banner);
			option_i.Find("option_canvas").Find("option_text").GetComponent<Text>().text = optionTexts[i];
			option_i.GetComponent<OptionBehavior>().myOption = options[i];
		}
		controller.chosenOption = "";

		float t = 0;
		while (t < 1) {
			t+=4f*Time.deltaTime;
			banner.position = Vector3.Lerp(new Vector3(-420f,316,-400),new Vector3(220f,316,-400),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		controller.locked = false;
		controller.nodrag = true;
		while (controller.chosenOption == "") {
			yield return 0;
			// Hopefully won't crash everything
		}
		controller.locked = true;
		controller.nodrag = false;
		t = 0;
		while (t < 1) {
			t+=4f*Time.deltaTime;
			banner.position = Vector3.Lerp(new Vector3(220f,316,-400),new Vector3(-420f,316,-400),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("option")) Destroy(g);
	}

	IEnumerator showRewards () {
		Transform banner = GameObject.Find("banner").transform;
		List<int> usedIndices = new List<int>();
		for (int i=0; i<4; i++) {
			Transform reward_i = (Transform)Instantiate(controller.card);
			reward_i.position = new Vector3 (-550+110*i,312,-410);
			reward_i.localScale = new Vector3(0.25f,0.25f,0.25f);
			reward_i.SetParent(banner);
			// Always provide a neuroendocrine stimulation card
			int index_i = controller.cardDictionary.cardTitles.Count-1;
			bool correctType = false;
			if (i > 0) {
				// Other cards should be distinct, non-effect cards
				while (!correctType) {
					index_i = UnityEngine.Random.Range(0,controller.cardDictionary.cardTitles.Count-1);
					if (!usedIndices.Contains(index_i) && controller.cardDictionary.cardTypes[index_i] != "effect") {
						correctType = true;
						usedIndices.Add(index_i);
					}
				}
			}
			CardBehavior cardbehavior_i = reward_i.GetComponent<CardBehavior>();
			cardbehavior_i.myTitle = controller.cardDictionary.cardTitles[index_i];
			cardbehavior_i.myText = controller.cardDictionary.cardTexts[index_i];
			cardbehavior_i.myTargets = controller.cardDictionary.cardTargets[index_i];
			cardbehavior_i.myDictionaryIndex = index_i;
			cardbehavior_i.tag = "reward_card";
			reward_i.Find("card_sprite").GetComponent<SpriteRenderer>().sprite = controller.cardDictionary.cardSprites[index_i];
			cardbehavior_i.refreshCard();
		}
		controller.chosenReward = -1;

		float t = 0;
		while (t < 1) {
			t+=4f*Time.deltaTime;
			banner.position = Vector3.Lerp(new Vector3(-420f,316,-400),new Vector3(220f,316,-400),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		controller.locked = false;
		controller.nodrag = true;
		while (controller.chosenReward == -1) {
			yield return 0;
			// Hopefully won't crash everything
		}
		controller.locked = true;
		controller.nodrag = false;
		t = 0;
		while (t < 1) {
			t+=4f*Time.deltaTime;
			banner.position = Vector3.Lerp(new Vector3(220f,316,-400),new Vector3(-420f,316,-400),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("reward_card")) Destroy(g);
		controller.provideCard(controller.chosenReward);
	}



	// --------------------------
	// Auxiliary Functions
	// --------------------------

	IEnumerator changeOpacity (float newValue) {
		float initValue = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color.a;
		Color fill = transform.Find("card_sprite").GetComponent<SpriteRenderer>().color;
		Color splineFill = transform.Find("card_spline").GetComponent<RageSpline>().fillColor1;
		float t=0;
		while (t < 1) {
			t+=6*Time.deltaTime;
			transform.Find("card_sprite").GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(fill.r,fill.g,fill.b,initValue),new Color(fill.r,fill.g,fill.b,newValue),Mathf.SmoothStep(0,1,t));
			transform.Find("card_spline").GetComponent<RageSpline>().fillColor1 = Color.Lerp(new Color(splineFill.r,splineFill.g,splineFill.b,initValue),new Color(splineFill.r,splineFill.g,splineFill.b,newValue),Mathf.SmoothStep(0,1,t));
			transform.Find("card_spline").GetComponent<RageSpline>().RefreshMesh();
			//GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}

	IEnumerator changeRot (Quaternion newRot) {
		Quaternion initRot = transform.rotation;
		float t=0;
		while (t < 1) {
			t+=6*Time.deltaTime;
			transform.rotation = Quaternion.Lerp(initRot,newRot,Mathf.SmoothStep(0,1,t));
			//GetComponent<RageSpline>().RefreshMesh(true,true,true);
			yield return 0;
		}
	}

	IEnumerator move (Vector3 newPos) {
		Vector3 initPos = transform.position;
		float t=0;
		while (t < 1) {
			t+=6*Time.deltaTime;
			transform.position = Vector3.Lerp(initPos,newPos,Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
	}

	public void refreshCard () {
		controller = GameObject.Find("Controller").GetComponent<Controller>();
		Debug.Log(myType);
		transform.Find("card_canvas").Find("card_title").GetComponent<Text>().text = myTitle;
		transform.Find("card_canvas").Find("card_text").GetComponent<Text>().text = myText;
		if (myType == "effect") {
			transform.Find("card_spline").GetComponent<RageSpline>().outlineColor1 = controller.effectOutline;
			transform.Find("card_spline").GetComponent<RageSpline>().fillColor1 = controller.effectColor1;
			transform.Find("card_spline").GetComponent<RageSpline>().fillColor2 = controller.effectColor2;
		} else {
			transform.Find("card_spline").GetComponent<RageSpline>().outlineColor1 = controller.hormoneOutline;
			transform.Find("card_spline").GetComponent<RageSpline>().fillColor1 = controller.hormoneColor1;
			transform.Find("card_spline").GetComponent<RageSpline>().fillColor2 = controller.hormoneColor2;
		}
		transform.Find("card_spline").GetComponent<RageSpline>().RefreshMesh();
	}


}
