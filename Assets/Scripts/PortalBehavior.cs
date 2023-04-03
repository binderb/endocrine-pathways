using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalBehavior : MonoBehaviour {

	public Camera portalCamera;
	public string myColliderObjectName;

	// Use this for initialization
	void Start () {



	}

	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)) {
      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

     	// do we hit our portal plane?
      if (Physics.Raycast(ray, out hit)) {
      	if (hit.collider.gameObject.name == myColliderObjectName) {
					// get texture coordinates of hit (without mesh collider)
        	Vector2 myPoint = new Vector2((ray.origin.x - transform.position.x)/GetComponent<RectTransform>().sizeDelta.x,1f+(ray.origin.y - transform.position.y)/GetComponent<RectTransform>().sizeDelta.y);
					Debug.Log(myPoint);
        	// convert the hit texture coordinates into camera coordinates
        	//Ray portalRay = portalCamera.ScreenPointToRay(Input.mousePosition);
					Ray portalRay = portalCamera.ScreenPointToRay(new Vector2(myPoint.x*portalCamera.pixelWidth,myPoint.y*portalCamera.pixelHeight));
					Debug.Log(portalRay);
					Debug.DrawRay(portalRay.origin,portalRay.direction*1000f,Color.red,2000,true);
					RaycastHit portalHit;


        	// test these camera coordinates in another raycast test
      		if(Physics.Raycast(portalRay, out portalHit)) {

						if (portalHit.collider.gameObject.GetComponent<Button>() != null) {
							portalHit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
						}
        	}
      	}
    	}
		}
	}

	void OnMouseDown () {
		/*//Vector3 localPoint = GameObject.Find("PSetView").transform.position + (Input.mousePosition - transform.position) + Vector3.back*400;
		Vector3 localPoint = hit.textureCoord;
		Ray portalRay = portalCamera.ScreenPointToRay(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight));
		//Ray portalRay = new Ray(localPoint,Vector3.forward);
		Debug.DrawLine(localPoint,localPoint+Vector3.forward*1000,Color.red,20,true);
		RaycastHit portalHit;
		if (Physics.Raycast(portalRay, out portalHit)) {
			if (portalHit.collider.gameObject.GetComponent<Button>() != null) {
				portalHit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
			}
		}*/
	}

}
