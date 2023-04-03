using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	public CardDictionary cardDictionary;
	GoalDictionary goalDictionary;
	int DECK_CAPACITY = 10;
	int HAND_INIT = 5;
	int HAND_CAPACITY = 7;
	public List<int> DECK = new List<int>();
	public List<int> HAND = new List<int>();
	public List<int> GOALS = new List<int>();

	public Transform card;
	public Transform sunburst;
	public Transform option;
	public Transform messageBox;
	public Transform instructionsBox;
	public Transform homeostasisLevel;
	public List<GoalBehavior> goalCards = new List<GoalBehavior>();

	public Color hormoneOutline;
	public Color hormoneColor1;
	public Color hormoneColor2;
	public Color effectOutline;
	public Color effectColor1;
	public Color effectColor2;

	public bool dragging;
	public bool zoomed;
	public bool locked;
	public bool nodrag;
	public bool message;
	public string chosenOption;
	public int chosenReward;
	public int goalsSatisfied = 0;
	public float homeostasis = 50f;
	public Vector3 dragInit;
	public Vector3 mouseInit;
	public Transform selectedCard;
	public RectTransform BigMessageCanvas;
	public string currentGender;
	public string currentSystem;

	public Text banner_title;
	public Text banner_subtitle;
	public Text banner_clickprompt;
	public Text banner_optionstitle;
	public Text BigMessageText1;
	public Text BigMessageText2;
	public Text maxText;
	public Text maxTextShadow;

	public delegate void stringVoidDelegate(string theString);
	public stringVoidDelegate buttonDelegate;
	public bool waitForUser;
	public bool waitForUI;


	// Use this for initialization
	void Start () {
		cardDictionary = GetComponent<CardDictionary>();
		goalDictionary = GetComponent<GoalDictionary>();
		banner_title = GameObject.Find("banner_title").GetComponent<Text>();
		banner_subtitle = GameObject.Find("banner_subtitle").GetComponent<Text>();
		banner_optionstitle = GameObject.Find("banner_optionstitle").GetComponent<Text>();
		banner_clickprompt = GameObject.Find("banner_clickprompt").GetComponent<Text>();
		BigMessageText1 = GameObject.Find("BigMessageText1").GetComponent<Text>();
		BigMessageText2 = GameObject.Find("BigMessageText2").GetComponent<Text>();
		maxText = GameObject.Find("MaxText").GetComponent<Text>();
		maxTextShadow = GameObject.Find("MaxTextShadow").GetComponent<Text>();
		goalCards.Add(GameObject.Find("goalCard1").GetComponent<GoalBehavior>());
		goalCards.Add(GameObject.Find("goalCard2").GetComponent<GoalBehavior>());
		goalCards.Add(GameObject.Find("goalCard3").GetComponent<GoalBehavior>());
		setDiagram("Female","Endocrine");
		locked = false;
		nodrag = false;
		goalDictionary.buildGoalDictionary();
	}

	// Update is called once per frame
	void Update () {
		if (dragging) {
			selectedCard.position = dragInit + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseInit);
		} else if (zoomed && (Input.GetMouseButtonDown(0))) {
			if (selectedCard.tag != "reward_card") {
				selectedCard.position = selectedCard.GetComponent<CardBehavior>().handPos;
				selectedCard.rotation = selectedCard.GetComponent<CardBehavior>().handRot;
				selectedCard.localScale = selectedCard.GetComponent<CardBehavior>().handScale;
			} else {
				Destroy(selectedCard.gameObject);
			}
			zoomed = false;
			locked = false;
		}
	}

	public void initGame () {
		StartCoroutine (initGameRoutine());
	}

	IEnumerator initGameRoutine () {
		locked = true;
		nodrag = true;
		// Hide goal cards
		foreach (GoalBehavior goalcard_i in goalCards) {
			goalcard_i.satisfied = false;
			goalcard_i.transform.localScale = new Vector3(0f,0.5f,0.5f);
			goalcard_i.mySlotSpline.outlineColor1 = new Color(0.36f,0.73f,1f,1.0f);
			goalcard_i.mySlotSpline.RefreshMesh(true,true,true);
		}
		// Move camera
		GameObject.Find("Main Camera").transform.position = GameObject.Find("Main Camera").transform.position + Vector3.forward*1000;
		//yield return new WaitForSeconds(0.5f);
		// Display begin message
		BigMessageText1.text = "Round 1";
		BigMessageText2.text = "Round 1";
		float t = 0;
		BigMessageCanvas.position = new Vector3(480,300,-499);
		SpriteRenderer dimPanel = BigMessageCanvas.Find("DimPanel").GetComponent<SpriteRenderer>();
		dimPanel.color = Color.clear;
		dimPanel.enabled = true;
		while (t < 1) {
			t+=2f*Time.deltaTime;
			dimPanel.color = Color.Lerp(Color.clear,new Color(0f,0f,0f,0.3f),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		BigMessageText1.color = Color.white;
		t = 0;
		while (t < 1) {
			t+=3f*Time.deltaTime;
			BigMessageText2.transform.localScale = Vector3.Lerp(Vector3.one,new Vector3(1.3f,2f,1.3f),Mathf.SmoothStep(0,1,t));
			BigMessageText2.color = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		yield return new WaitForSeconds(0.5f);
		t = 0;
		while (t < 1) {
			t+=3f*Time.deltaTime;
			BigMessageText1.color = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			dimPanel.color = Color.Lerp(new Color(0f,0f,0f,0.3f),Color.clear,Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		BigMessageCanvas.position = new Vector3(480,300,100);
		// Visually initialize UI
		setHomeostasis(25f);
		buildDecks();
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(dealNewHandRoutine());
		locked = false;
		nodrag = false;
		//showInstructions();
	}

	// ---------------------------
	// Deck Building Functions
	// ---------------------------

	void buildDecks () {
		// Build hormone deck
		DECK = new List<int>();
		for (int i=0;i<1;i++) DECK.Add(0); // GnRH x 1
		for (int i=0;i<1;i++) DECK.Add(1); // TRH x 1
		for (int i=0;i<1;i++) DECK.Add(2); // GHRH x 1
		for (int i=0;i<1;i++) DECK.Add(3); // CRH x 1
		for (int i=0;i<1;i++) DECK.Add(4); // LH x 1
		for (int i=0;i<1;i++) DECK.Add(5); // FSH x 1
		for (int i=0;i<1;i++) DECK.Add(6); // GH x 1
		for (int i=0;i<1;i++) DECK.Add(14); // glucagon x 1
		for (int i=0;i<1;i++) DECK.Add(15); // insulin x 1
		for (int i=0;i<1;i++) DECK.Add(8); // ACTH x 1
		for (int i=0;i<1;i++) DECK.Add(10); // calcitonin x 1
		for (int i=0;i<1;i++) DECK.Add(11); // PTH x 1
		// Shuffle
		for (int i = 0; i < DECK.Count; i++) {
      int temp = DECK[i];
      int randomIndex = Random.Range(i, DECK.Count);
      DECK[i] = DECK[randomIndex];
      DECK[randomIndex] = temp;
    }

		// Hide goal cards
		foreach (GoalBehavior goalcard_i in goalCards) {
			goalcard_i.satisfied = false;
			goalcard_i.transform.localScale = new Vector3(0f,0.5f,0.5f);
			goalcard_i.mySlotSpline.outlineColor1 = new Color(0.36f,0.73f,1f,1.0f);
			goalcard_i.mySlotSpline.RefreshMesh(true,true,true);
		}
		// Build goal deck
		GOALS = new List<int>();
		for (int i=0;i<1;i++) GOALS.Add(0); // Temp up x 1
		for (int i=0;i<1;i++) GOALS.Add(1); // Glucose up x 1
		for (int i=0;i<1;i++) GOALS.Add(2); // Glucose down x 1
		for (int i=0;i<1;i++) GOALS.Add(3); // Estradiol x 1
		for (int i=0;i<1;i++) GOALS.Add(4); // Testosterone x 1
		for (int i=0;i<1;i++) GOALS.Add(5); // Ovulation x 1
		for (int i=0;i<1;i++) GOALS.Add(6); // Spermatogenesis x 1
		for (int i=0;i<1;i++) GOALS.Add(7); // Fatty acids x 1
		for (int i=0;i<1;i++) GOALS.Add(8); // Water up x 1
		for (int i=0;i<1;i++) GOALS.Add(9); // Calcium up x 1
		for (int i=0;i<1;i++) GOALS.Add(10); // Calcium down x 1
		for (int i=0;i<1;i++) GOALS.Add(11); // Uterine contractions x 1
		// Shuffle
		for (int i = 0; i < GOALS.Count; i++) {
      int temp = GOALS[i];
      int randomIndex = Random.Range(i, GOALS.Count);
      GOALS[i] = GOALS[randomIndex];
      GOALS[randomIndex] = temp;
    }
	}

	// ---------------------------
	// Gameplay Functions
	// ---------------------------

	public void dealNewHand () {
		buildDecks();
		StartCoroutine(dealNewHandRoutine());
	}

	IEnumerator dealNewHandRoutine () {
		locked = true;
		// Pick and show goal cards
		for (int i=0; i<3; i++) {
			StartCoroutine(showGoal(i,GOALS[0]));
			GOALS.RemoveAt(0);
			yield return new WaitForSeconds(0.3f);
		}
		goalsSatisfied = 0;

		HAND = new List<int>();
		// Guarantee the player a neuroendocrine stimulation card
		HAND.Add(cardDictionary.cardTitles.Count-1);
		// Draw the rest of the hand
		for (int i=0; i<HAND_INIT-1; i++) {
			HAND.Add(DECK[0]);
			DECK.RemoveAt(0);
			displayHand();
			yield return new WaitForSeconds(0.1f);
		}
		locked = false;
	}

	IEnumerator showGoal (int index, int newGoal) {
		locked = true;
		goalCards[index].myTitle = goalDictionary.goalTitles[newGoal];
		goalCards[index].myText = goalDictionary.goalTexts[newGoal];
		goalCards[index].mySolution = goalDictionary.goalSolutions[newGoal];
		goalCards[index].GetComponent<SpriteRenderer>().sprite = goalDictionary.goalSprites[goalDictionary.goalSprites.Count-1];
		goalCards[index].transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		float t = 0;
		while (t < 1) {
			t+=8f*Time.deltaTime;
			goalCards[index].transform.localScale = Vector3.Lerp(new Vector3(0.5f,0.5f,0.5f),new Vector3(0f,0.5f,0.5f),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		if (goalDictionary.goalSpriteIndices.Count >= newGoal+1) {goalCards[index].GetComponent<SpriteRenderer>().sprite = goalDictionary.goalSprites[goalDictionary.goalSpriteIndices[newGoal]];}
		t = 0;
		while (t < 1) {
			t+=8f*Time.deltaTime;
			goalCards[index].transform.localScale = Vector3.Lerp(new Vector3(0f,0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		locked = false;
	}

	public void provideCard (int cardIndex) {
		HAND.Add(cardIndex);
		displayHand();
	}

	public void displayHand () {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("card")) Destroy(g);
		for (int i=0;i<HAND.Count;i++) {
			Transform card_i = (Transform)Instantiate(card);
			CardBehavior cardbehavior_i = card_i.GetComponent<CardBehavior>();
			cardbehavior_i.myTitle = cardDictionary.cardTitles[HAND[i]];
			cardbehavior_i.myText = cardDictionary.cardTexts[HAND[i]];
			cardbehavior_i.myTargets = cardDictionary.cardTargets[HAND[i]];
			cardbehavior_i.myType = cardDictionary.cardTypes[HAND[i]];
			cardbehavior_i.myDictionaryIndex = HAND[i];
			cardbehavior_i.handIndex = i;
			if (cardDictionary.cardSprites.Count >= HAND[i]+1) {card_i.Find("card_sprite").GetComponent<SpriteRenderer>().sprite = cardDictionary.cardSprites[HAND[i]];}
			cardbehavior_i.refreshCard();
			card_i.localScale = new Vector3(0.4f,0.4f,0.4f);
			Vector3 handCenter = new Vector3(300,50,-201);
			float cardSpread = 80f;
			//card_i.position = new Vector3((handCenter.x - (HAND.Count/2f)*50)+i*50,50-5*Mathf.Abs(i-(Mathf.Floor(HAND_INIT/2f))),-2*(i+1));
			card_i.position = new Vector3((handCenter.x - (HAND.Count/2f)*cardSpread)+i*cardSpread,handCenter.y-10*Mathf.Abs(i-(Mathf.Floor(HAND.Count/2f))),handCenter.z+(-2*(i+1)));
			card_i.rotation = Quaternion.Euler(0f,0f,(5f*HAND.Count/2f)-i*5f);
			cardbehavior_i.handPos = card_i.position;
		}
	}

	public void hideHand () {
		StartCoroutine(hideHandRoutine());
	}

	IEnumerator hideHandRoutine () {
		float t = 0;
		while (t < 1) {
			t+=3*Time.deltaTime;
			foreach (GameObject card_i in GameObject.FindGameObjectsWithTag("card")) {
				card_i.transform.position = Vector3.Lerp(card_i.GetComponent<CardBehavior>().handPos,card_i.GetComponent<CardBehavior>().handPos+Vector3.down*200,Mathf.SmoothStep(0,1,t));
			}
			yield return 0;
		}
	}

	public void reset () {
		if (!locked && !nodrag) StartCoroutine(resetRoutine());
	}

	IEnumerator resetRoutine() {
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().alpha = 1f;
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().interactable = true;
		messageBox.Find("ResetCanvas").Find("Yes").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = true;
		messageBox.Find("ResetCanvas").Find("No").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = true;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().alpha = 0f;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().interactable = false;
		messageBox.Find("IncorrectCanvas").Find("Ok").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("IncorrectCanvas").Find("Explain").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().alpha = 0f;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().interactable = false;
		messageBox.Find("ExplainCanvas").Find("Ok").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		locked = true;
		nodrag = true;
		float t = 0;
		while (t < 1) {
			t+=4*Time.deltaTime;
			messageBox.position = Vector3.Lerp(new Vector3(480,900,-498),new Vector3(480,300,-498),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
	}

	public void dismissMessageBox () {
		StartCoroutine(dismissMessageBoxRoutine());
		// Get rid of any exempt cards—this is for when the user dismisses an "incorrect target" message
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("exempt_card")) Destroy(g);
	}

	IEnumerator dismissMessageBoxRoutine() {
		float t = 0;
		while (t < 1) {
			t+=4*Time.deltaTime;
			messageBox.position = Vector3.Lerp(new Vector3(480,300,-498),new Vector3(480,900,-498),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		locked = false;
		nodrag = false;
		message = false;
	}

	public void confirmReset () {
		StartCoroutine(confirmResetRoutine());
	}

	IEnumerator confirmResetRoutine() {
		StartCoroutine(hideHandRoutine());
		float t = 0;
		while (t < 1) {
			t+=4*Time.deltaTime;
			messageBox.position = Vector3.Lerp(new Vector3(480,300,-498),new Vector3(480,900,-498),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		yield return StartCoroutine(changeFlash(-5));
		setHomeostasis(homeostasis-5f);
		dealNewHand();
		locked = false;
		nodrag = false;
		message = false;
	}

	public void completeRound () {
		StartCoroutine(completeRoundRoutine());
	}

	IEnumerator completeRoundRoutine () {
		float t = 0;
		BigMessageCanvas.position = new Vector3(480,300,-499);
		SpriteRenderer dimPanel = BigMessageCanvas.Find("DimPanel").GetComponent<SpriteRenderer>();
		dimPanel.color = Color.clear;
		dimPanel.enabled = true;
		BigMessageText1.text = "Round Complete!";
		BigMessageText2.text = "Round Complete!";
		while (t < 1) {
			t+=2f*Time.deltaTime;
			dimPanel.color = Color.Lerp(Color.clear,new Color(0f,0f,0f,0.3f),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		BigMessageText1.color = Color.white;
		t = 0;
		while (t < 1) {
			t+=3f*Time.deltaTime;
			BigMessageText2.transform.localScale = Vector3.Lerp(Vector3.one,new Vector3(1.3f,2f,1.3f),Mathf.SmoothStep(0,1,t));
			BigMessageText2.color = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		yield return new WaitForSeconds(0.5f);
		t = 0;
		while (t < 1) {
			t+=3f*Time.deltaTime;
			BigMessageText1.color = Color.Lerp(Color.white,new Color(1f,1f,1f,0f),Mathf.SmoothStep(0,1,t));
			dimPanel.color = Color.Lerp(new Color(0f,0f,0f,0.3f),Color.clear,Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		BigMessageCanvas.position = new Vector3(480,300,100);
		yield return StartCoroutine(changeFlash(20));
		setHomeostasis(homeostasis+20f);
		dealNewHand();
	}

	public void showIncorrectMessage (int badIndex) {
		StartCoroutine(showIncorrectMessageRoutine(badIndex));
	}

	IEnumerator showIncorrectMessageRoutine (int badIndex) {
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().alpha = 0f;
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().interactable = false;
		messageBox.Find("ResetCanvas").Find("Yes").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("ResetCanvas").Find("No").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().alpha = 1f;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().interactable = true;
		messageBox.Find("IncorrectCanvas").Find("Ok").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = true;
		messageBox.Find("IncorrectCanvas").Find("Explain").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = true;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().alpha = 0f;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().interactable = false;
		messageBox.Find("ExplainCanvas").Find("Ok").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		locked = true;
		nodrag = true;
		message = true;
		float t = 0;
		while (t < 1) {
			t+=4*Time.deltaTime;
			messageBox.position = Vector3.Lerp(new Vector3(480,900,-498),new Vector3(480,300,-498),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		while (message) {
			yield return 0;
		}
		yield return StartCoroutine(changeFlash(-2));
		setHomeostasis(homeostasis-2f);
		provideCard(badIndex);
	}

	public void showExplanation () {
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().alpha = 0f;
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
		messageBox.Find("ResetCanvas").GetComponent<CanvasGroup>().interactable = false;
		messageBox.Find("ResetCanvas").Find("Yes").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("ResetCanvas").Find("No").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().alpha = 0f;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
		messageBox.Find("IncorrectCanvas").GetComponent<CanvasGroup>().interactable = false;
		messageBox.Find("IncorrectCanvas").Find("Ok").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("IncorrectCanvas").Find("Explain").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = false;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().alpha = 1f;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
		messageBox.Find("ExplainCanvas").GetComponent<CanvasGroup>().interactable = true;
		messageBox.Find("ExplainCanvas").Find("Ok").Find("ButtonSpline").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("ExplainText").GetComponent<Text>().text = cardDictionary.cardHints[selectedCard.GetComponent<CardBehavior>().myDictionaryIndex];
	}

	public void showInstructions () {
		if (!locked && !nodrag) {
			locked = true;
			nodrag = true;
			instructionsBox.position = new Vector3(480,300,-498);
		}
	}

	public void hideInstructions () {
		locked = false;
		nodrag = false;
		instructionsBox.position = new Vector3(480,300,100);
	}

	// ---------------------------
	// Auxiliary Functions
	// ---------------------------

	public void setHomeostasis (float newValue) {
		if (newValue < 0) newValue = 0;
		if (newValue > 100) newValue = 100;
		homeostasis = newValue;
		StartCoroutine(setHomeostasisRoutine(newValue));
	}

	IEnumerator setHomeostasisRoutine (float newValue) {
		float minVal = -457f;
		float maxVal = 37f;
		Vector3 initPos = homeostasisLevel.localPosition;
		Vector3 finalPos = new Vector3(-457f+(maxVal-minVal)*(newValue/100f),initPos.y,initPos.z);
		if (newValue < 100) {
			maxText.enabled = false;
			maxTextShadow.enabled = false;
		}
		float t = 0;
		while (t < 1) {
			t+=1*Time.deltaTime;
			homeostasisLevel.localPosition = Vector3.Lerp(initPos,finalPos,Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		if (newValue == 100) {
			maxText.enabled = true;
			maxTextShadow.enabled = true;
		}
	}

	IEnumerator changeFlash (int change) {
		Color myColor = new Color(0.36f,0.73f,1f,1.0f);
		Color myTransparentColor = new Color(0.36f,0.73f,1f,0f);
		Color myAuraColor = Color.white;
		Color myAuraTransparentColor = new Color(1f,1f,1f,0f);
		if (change < 0) {
			myColor = Color.red;
			myTransparentColor = new Color(1f,0f,0f,0f);
			myAuraColor = Color.red;
			myAuraTransparentColor = new Color(1f,0f,0f,0f);
		}
		Transform changeNumbers = GameObject.Find("ChangeNumbers").transform;
		Text foregroundText = GameObject.Find("Foreground").GetComponent<Text>();
		Text backgroundText = GameObject.Find("Background").GetComponent<Text>();
		SpriteRenderer changeAura = GameObject.Find("ChangeFlash").GetComponent<SpriteRenderer>();
		changeAura.enabled = true;
		changeNumbers.position = new Vector3(480,300,-499);
		foregroundText.text = change + "% Homeostasis";
		backgroundText.text = change + "% Homeostasis";
		foregroundText.color = myColor;
		backgroundText.color = Color.black;
		foregroundText.enabled = true;
		backgroundText.enabled = true;
		float t = 0;
		while (t < 1) {
			t+=2*Time.deltaTime;
			changeAura.color = Color.Lerp(myAuraColor,myAuraTransparentColor,Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		changeAura.enabled = false;
		t = 0;
		while (t < 1) {
			t+=2*Time.deltaTime;
			changeNumbers.position = Vector3.Lerp(new Vector3(480,300,-499),new Vector3(350,500,-499),Mathf.SmoothStep(0,1,t));
			foregroundText.color = Color.Lerp(myColor,myTransparentColor,Mathf.SmoothStep(0,1,t));
			backgroundText.color = Color.Lerp(Color.black,new Color(0f,0f,0f,0f),Mathf.SmoothStep(0,1,t));
			yield return 0;
		}
		foregroundText.enabled = true;
		backgroundText.enabled = true;
	}

	public void toggleGender () {
		if (!locked && !nodrag) {
			Text genderText = GameObject.Find("GenderToggle").transform.Find("Text").GetComponent<Text>();
			if (genderText.text == "Female") {
				genderText.text = "Male";
				setDiagram("Male",currentSystem);
			} else {
				genderText.text = "Female";
				setDiagram("Female",currentSystem);
			}
		}
	}

	public void toggleSystem () {
		if (!locked && !nodrag) {
			Text systemText = GameObject.Find("SystemToggle").transform.Find("Text").GetComponent<Text>();
			if (systemText.text == "Endocrine") {
				systemText.text = "Other";
				setDiagram(currentGender,"Other");
			} else {
				systemText.text = "Endocrine";
				setDiagram(currentGender,"Endocrine");
			}
		}
	}

	void setDiagram (string gender, string system) {
		if (gender == "Female") {
			currentGender = "Female";
			GameObject.Find("female_outline").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find("male_outline").GetComponent<MeshRenderer>().enabled = false;
			if (system == "Endocrine") {
				currentSystem = "Endocrine";
				GameObject.Find("female_endocrine_text").GetComponent<CanvasGroup>().alpha = 1;
				GameObject.Find("female_somatic_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("male_endocrine_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("male_somatic_text").GetComponent<CanvasGroup>().alpha = 0;

				foreach (Transform organ_i in GameObject.Find("female_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = true;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = true;
						organ_i.GetComponent<BoxCollider>().enabled = true;
					}
				}
				foreach (Transform organ_i in GameObject.Find("female_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}

			} else if (system == "Other") {
				currentSystem = "Other";
				GameObject.Find("female_endocrine_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("female_somatic_text").GetComponent<CanvasGroup>().alpha = 1;
				GameObject.Find("male_endocrine_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("male_somatic_text").GetComponent<CanvasGroup>().alpha = 0;

				foreach (Transform organ_i in GameObject.Find("female_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("female_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = true;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = true;
						organ_i.GetComponent<BoxCollider>().enabled = true;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
			}

		} else if (gender == "Male") {
			currentGender = "Male";
			GameObject.Find("female_outline").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("male_outline").GetComponent<MeshRenderer>().enabled = true;
			if (system == "Endocrine") {
				currentSystem = "Endocrine";
				GameObject.Find("female_endocrine_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("female_somatic_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("male_endocrine_text").GetComponent<CanvasGroup>().alpha = 1;
				GameObject.Find("male_somatic_text").GetComponent<CanvasGroup>().alpha = 0;

				foreach (Transform organ_i in GameObject.Find("female_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("female_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = true;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = true;
						organ_i.GetComponent<BoxCollider>().enabled = true;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}

			} else if (system == "Other") {
				currentSystem = "Other";
				GameObject.Find("female_endocrine_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("female_somatic_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("male_endocrine_text").GetComponent<CanvasGroup>().alpha = 0;
				GameObject.Find("male_somatic_text").GetComponent<CanvasGroup>().alpha = 1;

				foreach (Transform organ_i in GameObject.Find("female_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}

				}
				foreach (Transform organ_i in GameObject.Find("female_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_endocrine_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = false;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = false;
						organ_i.GetComponent<BoxCollider>().enabled = false;
					}
				}
				foreach (Transform organ_i in GameObject.Find("male_somatic_organs").transform) {
					if (organ_i.GetComponent<SpriteRenderer>() != null) organ_i.GetComponent<SpriteRenderer>().enabled = true;
					else {
						organ_i.GetComponent<MeshRenderer>().enabled = true;
						organ_i.GetComponent<BoxCollider>().enabled = true;
					}
				}
			}

		}
	}

	public Vector3 identifyPosition (Text theText, int charIndex) {

		string text = theText.text;

		TextGenerator textGen = new TextGenerator (text.Length);
		Vector2 extents = theText.gameObject.GetComponent<RectTransform>().rect.size;
		textGen.Populate (text, theText.GetGenerationSettings (extents));

		int newLine = text.Substring(0, charIndex).Split('\n').Length - 1;
		int indexOfTextQuad = (charIndex * 4) + (newLine * 4) - 4;
		if (indexOfTextQuad < textGen.vertexCount) {
			Vector3 avgPos = (textGen.verts[indexOfTextQuad].position +
			textGen.verts[indexOfTextQuad + 1].position +
			textGen.verts[indexOfTextQuad + 2].position +
			textGen.verts[indexOfTextQuad + 3].position) / 4f;
			Vector3 worldPos = theText.transform.TransformPoint (avgPos);
			return worldPos;
		} else {
			Debug.LogError ("Out of text bound");
			return Vector3.zero;
		}
	}

	public float getTextWidth(Text theText) {
		Vector3 leftBound = identifyPosition(theText,1);
		Vector3 rightBound = identifyPosition(theText,theText.text.Length);
		return Mathf.Abs(leftBound.x-rightBound.x);
	}


}
