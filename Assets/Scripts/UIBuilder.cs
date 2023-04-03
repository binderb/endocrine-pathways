using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilder : MonoBehaviour {

	/* Trying to make a generic class for building mobile UI elements.
	 * Needs the following prefabs to function properly:
	 *
	 * - 1 8-sided RageSpline to serve as the roundrect base
	 * - 1 UIButton (including text) with all sprite info cleared and a built-in canvas
	 *
	 */

	public Transform roundRectBase;
	public Transform buttonBase;
	public Transform textBase;
	public Transform multilineTextBase;
	public Transform scrollViewBase;
	public Controller controller;

	public Color UIBase;
	public Color UIHighlight;
	public Color UIDialog;
	public Color darkUI;
	public Color lightUI;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller>();
	}

	// Update is called once per frame
	void Update () {

	}

	// ---------------------------------------
	// Builder Functions
	// ---------------------------------------

	public Transform buildRoundRect (Vector3 myPos, float myWidth, float myHeight, float cornerRadius) {

		Transform myRect = (Transform)Instantiate(roundRectBase);
		myRect.position = myPos;

		Vector3 topLeftCorner = myRect.position - Vector3.right*(myWidth/2f) + Vector3.up*(myHeight/2f);
		Vector3 topRightCorner = myRect.position + Vector3.right*(myWidth/2f) + Vector3.up*(myHeight/2f);
		Vector3 bottomLeftCorner = myRect.position - Vector3.right*(myWidth/2f) - Vector3.up*(myHeight/2f);
		Vector3 bottomRightCorner = myRect.position + Vector3.right*(myWidth/2f) - Vector3.up*(myHeight/2f);

		RageSpline mySpline = myRect.GetComponent<RageSpline>();
		mySpline.SetPointWorldSpace(0,(topLeftCorner - Vector3.up*cornerRadius),Vector3.zero,Vector3.up*(2*cornerRadius/3f));
		mySpline.SetPointWorldSpace(1,(topLeftCorner + Vector3.right*cornerRadius),Vector3.left*(2*cornerRadius/3f),Vector3.zero);
		mySpline.SetPointWorldSpace(2,(topRightCorner - Vector3.right*cornerRadius),Vector3.zero,Vector3.right*(2*cornerRadius/3f));
		mySpline.SetPointWorldSpace(3,(topRightCorner - Vector3.up*cornerRadius),Vector3.up*(2*cornerRadius/3f),Vector3.zero);
		mySpline.SetPointWorldSpace(4,(bottomRightCorner + Vector3.up*cornerRadius),Vector3.zero,Vector3.down*(2*cornerRadius/3f));
		mySpline.SetPointWorldSpace(5,(bottomRightCorner - Vector3.right*cornerRadius),Vector3.right*(2*cornerRadius/3f),Vector3.zero);
		mySpline.SetPointWorldSpace(6,(bottomLeftCorner + Vector3.right*cornerRadius),Vector3.zero,Vector3.left*(2*cornerRadius/3f));
		mySpline.SetPointWorldSpace(7,(bottomLeftCorner + Vector3.up*cornerRadius),Vector3.down*(2*cornerRadius/3f),Vector3.zero);
		mySpline.RefreshMesh();

		return myRect;
	}

	public Transform buildButton (string myName, string myText, Controller.stringVoidDelegate myDelegate, string delegateString, Vector3 myPos, Vector2 mySize, float cornerRadius, int myFontSize) {

		Transform myObject = (Transform)Instantiate(buttonBase);
		myObject.gameObject.name = myName;
		myObject.transform.position = myPos;
		myObject.GetComponent<RectTransform>().sizeDelta = mySize;
		myObject.GetComponent<BoxCollider>().size = new Vector3(mySize.x,mySize.y,1f);
		myObject.Find("ButtonCanvas").Find("ButtonText").GetComponent<Text>().text = myText;
		myObject.Find("ButtonCanvas").Find("ButtonText").GetComponent<Text>().fontSize = myFontSize;
		//myObject.Find("ButtonCanvas").GetComponent<RectTransform>().sizeDelta = mySize;
		//myObject.Find("ButtonCanvas").Find("ButtonText").GetComponent<RectTransform>().sizeDelta = mySize;


		Transform myRect = buildRoundRect(myPos,mySize.x,mySize.y,cornerRadius);
		myRect.gameObject.name = "ButtonSpline";
		myRect.GetComponent<RageSpline>().fillColor1 = UIBase;
		myRect.GetComponent<RageSpline>().RefreshMesh();
		myRect.SetParent(myObject);

		myObject.GetComponent<Button>().onClick.AddListener(delegate{myDelegate(delegateString);});
		//myObject.GetComponent<Button>().onClick.AddListener(delegate{testListener("You have clicked "+myObject.name+"!");});
		myObject.GetComponent<Button>().onClick.AddListener(delegate{popButton(myObject);});
		myObject.GetComponent<Button>().onClick.AddListener(delegate{highlightButton(myObject);});
		//myObject.GetComponent<Button>().onClick.AddListener(delegate{highlightFadeButton(myObject);});

		return myObject;
	}

	public Transform buildText (string myText, Color myColor, Vector3 myPos, string myAlignment, int myFontSize, bool initHidden) {
		Transform myCanvas = (Transform)Instantiate(textBase);
		Text myTextObject = myCanvas.Find("TextBaseText").GetComponent<Text>();
		switch (myAlignment) {
		case "center":
			myTextObject.transform.localPosition = new Vector3(0f,0f,0f);
			break;
		case "left":
			myCanvas.GetComponent<RectTransform>().anchorMin = new Vector2(0f,0.5f);
			myCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(0f,0.5f);
			myCanvas.GetComponent<RectTransform>().pivot = new Vector2(0f,0.5f);
			myTextObject.alignment = TextAnchor.MiddleLeft;
			break;
		case "right":
			myCanvas.GetComponent<RectTransform>().anchorMin = new Vector3(1f,0.5f);
			myCanvas.GetComponent<RectTransform>().anchorMax = new Vector3(1f,0.5f);
			myCanvas.GetComponent<RectTransform>().pivot = new Vector2(1f,0.5f);
			myTextObject.alignment = TextAnchor.MiddleRight;
			break;
		default:
			break;
		}
		myTextObject.text = myText;
		myTextObject.fontSize = myFontSize;
		myTextObject.color = myColor;
		myCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(controller.getTextWidth(myTextObject)+9f,100f);
		myCanvas.position = myPos;
		if (initHidden) myCanvas.GetComponent<CanvasGroup>().alpha = 0f;

		return myCanvas;
	}

	public Transform buildMultilineText (string myText, Color myColor, Vector3 myPos, Vector2 mySize, string myAlignment, int myFontSize, bool initHidden) {
		Transform myCanvas = (Transform)Instantiate(multilineTextBase);
		Text myTextObject = myCanvas.Find("MultilineTextBaseText").GetComponent<Text>();
		switch (myAlignment) {
		case "center":
			myTextObject.transform.localPosition = new Vector3(0f,0f,0f);
			myTextObject.alignment = TextAnchor.UpperCenter;
			break;
		case "left":
			myCanvas.GetComponent<RectTransform>().anchorMin = new Vector2(0f,0.5f);
			myCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(0f,0.5f);
			myCanvas.GetComponent<RectTransform>().pivot = new Vector2(0f,0.5f);
			myTextObject.alignment = TextAnchor.UpperLeft;
			break;
		case "right":
			myCanvas.GetComponent<RectTransform>().anchorMin = new Vector3(1f,0.5f);
			myCanvas.GetComponent<RectTransform>().anchorMax = new Vector3(1f,0.5f);
			myCanvas.GetComponent<RectTransform>().pivot = new Vector2(1f,0.5f);
			myTextObject.alignment = TextAnchor.UpperRight;
			break;
		default:
			break;
		}
		myTextObject.text = myText;
		myTextObject.fontSize = myFontSize;
		myTextObject.color = myColor;
		myCanvas.GetComponent<RectTransform>().sizeDelta = mySize;
		myCanvas.position = myPos;
		if (initHidden) myCanvas.GetComponent<CanvasGroup>().alpha = 0f;

		return myCanvas;
	}

	public Transform buildDialog (string myTitle, string myContent, Vector3 myPos, Vector2 mySize) {

		Transform myBox = buildRoundRect(myPos, mySize.x, mySize.y, 25f);
		myBox.GetComponent<RageSpline>().fillColor1 = UIDialog;
		myBox.GetComponent<RageSpline>().RefreshMesh();
		Transform myOkButton = buildButton("okbutton", "OK", controller.buttonDelegate, "", myPos + Vector3.right*(mySize.x/2f-80f) + Vector3.down*(mySize.y/2f-50f) + Vector3.back, new Vector2(120f,60f), 25f, 24);
		myOkButton.SetParent(myBox);
		Transform theTitle = buildText(myTitle,lightUI,myPos + Vector3.left*(mySize.x/2f-20f) + Vector3.up*(mySize.y/2f-60f) + Vector3.back,"left",36,false);
		theTitle.SetParent(myBox);
		Transform theContent = buildMultilineText(myContent,lightUI,myPos + Vector3.left*(mySize.x/2f-20f) + Vector3.down*(20f) + Vector3.back,new Vector2(mySize.x-40f,mySize.y-100f),"left",20,false);
		theContent.SetParent(myBox);

		return myBox;

	}

	public Transform buildProjectedScrollView (string objectPrefix,
																						 Vector3 myPos,
																						 Vector2 mySize,
																						 Vector3 projectionPos,
																						 Vector2 projectionSize) {

		Transform myScrollView = (Transform)Instantiate(scrollViewBase);
		Transform myViewport = myScrollView.Find("ScrollViewportBase");
		Transform myContent = myScrollView.Find("ScrollViewportBase").Find("ScrollContentBase");
		Transform myImage = myScrollView.Find("ScrollViewportBase").Find("ScrollContentBase").Find("ProjectedImageBase");
		Camera myCamera = myScrollView.Find("ScrollCameraBase").GetComponent<Camera>();
		myScrollView.name = objectPrefix + "ScrollView";
		myViewport.name = objectPrefix + "ScrollViewport";
		myContent.name = objectPrefix + "ScrollContent";
		myImage.name = objectPrefix + "ScrollImage";
		myCamera.name = objectPrefix + "ScrollCamera";
		myScrollView.position = myPos;
		myScrollView.GetComponent<RectTransform>().sizeDelta = mySize;
		myScrollView.GetComponent<BoxCollider>().size = new Vector3(mySize.x,mySize.y,1f);
		myScrollView.GetComponent<BoxCollider>().center = new Vector3(mySize.x/2f,mySize.y/2f,0f);
		myContent.GetComponent<RectTransform>().sizeDelta = projectionSize;
		myImage.GetComponent<RectTransform>().sizeDelta = projectionSize;
		myCamera.transform.position = projectionPos;
		myCamera.aspect = projectionSize.x/projectionSize.y;
		myCamera.orthographicSize = projectionSize.y/2f;
		myCamera.targetTexture = new RenderTexture((int)projectionSize.x,(int)projectionSize.y,24);
		myCamera.clearFlags = CameraClearFlags.SolidColor;
		myCamera.backgroundColor = Color.clear;
		myImage.GetComponent<RawImage>().texture = myCamera.targetTexture;
		myImage.GetComponent<PortalBehavior>().myColliderObjectName = myScrollView.name;

		return myScrollView;
	}

	// ---------------------------------------
	// Cosmetic Functions
	// ---------------------------------------

	public void testListener (string myMessage) {
		Debug.Log(myMessage);
		controller.waitForUser = false;
	}

	public void highlightFadeButton (Transform theButton) {
		StartCoroutine(highlightButtonRoutine(theButton));
	}

	IEnumerator highlightFadeButtonRoutine (Transform theButton) {
		float t = 0;
		while (t < 1) {
			t += 3f*Time.deltaTime;
			theButton.Find("ButtonSpline").GetComponent<RageSpline>().fillColor1 = Color.Lerp(UIHighlight,new Color(UIBase.r,UIBase.g,UIBase.b,0f),Mathf.SmoothStep(0f,1f,t));
			theButton.Find("ButtonSpline").GetComponent<RageSpline>().RefreshMesh();
			yield return 0;
		}
	}

	public void highlightButton (Transform theButton) {
		StartCoroutine(highlightButtonRoutine(theButton));
	}

	IEnumerator highlightButtonRoutine (Transform theButton) {
		float t = 0;
		while (t < 1) {
			t += 8f*Time.deltaTime;
			theButton.Find("ButtonSpline").GetComponent<RageSpline>().fillColor1 = Color.Lerp(UIHighlight,UIBase,Mathf.SmoothStep(0f,1f,t));
			theButton.Find("ButtonSpline").GetComponent<RageSpline>().RefreshMesh();
			yield return 0;
		}
	}

	public void popButton (Transform theButton) {
		StartCoroutine(popButtonRoutine(theButton));
	}

	IEnumerator popButtonRoutine (Transform theButton) {
		float t = 0;
		while (t < 1) {
			t += 10f*Time.deltaTime;
			theButton.localScale = Vector3.Lerp(new Vector3(1.3f,1.3f,1f),Vector3.one,Mathf.SmoothStep(0f,1f,t));
			yield return 0;
		}
		t = 0;
		while (t < 1) {
			t += 10f*Time.deltaTime;
			theButton.localScale = Vector3.Lerp(Vector3.one,new Vector3(1.05f,1.05f,1f),Mathf.SmoothStep(0f,1f,t));
			yield return 0;
		}
		t = 0;
		while (t < 1) {
			t += 10f*Time.deltaTime;
			theButton.localScale = Vector3.Lerp(new Vector3(1.05f,1.05f,1f),Vector3.one,Mathf.SmoothStep(0f,1f,t));
			yield return 0;
		}
		controller.waitForUI = false;
	}

}
