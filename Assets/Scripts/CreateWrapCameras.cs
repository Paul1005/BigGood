using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWrapCameras : MonoBehaviour {

	public Camera wrapCamera;
	private Vector3 horizontalOffset;
	private Vector3 verticalOffset;

	// Use this for initialization
	void Start () {
		horizontalOffset = Camera.main.ViewportToWorldPoint(new Vector3(1.5f, 0.5f, -Camera.main.transform.position.z));
		verticalOffset = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.5f, -Camera.main.transform.position.z));

		// Edges
		Instantiate (wrapCamera, transform.position + horizontalOffset, wrapCamera.transform.rotation);
		Instantiate (wrapCamera, transform.position - horizontalOffset, wrapCamera.transform.rotation);
		Instantiate (wrapCamera, transform.position + verticalOffset, wrapCamera.transform.rotation);
		Instantiate (wrapCamera, transform.position - verticalOffset, wrapCamera.transform.rotation);

		// Corners
		Instantiate (wrapCamera, transform.position + horizontalOffset + verticalOffset, wrapCamera.transform.rotation);
		Instantiate (wrapCamera, transform.position - horizontalOffset + verticalOffset, wrapCamera.transform.rotation);
		Instantiate (wrapCamera, transform.position + horizontalOffset - verticalOffset, wrapCamera.transform.rotation);
		Instantiate (wrapCamera, transform.position - horizontalOffset - verticalOffset, wrapCamera.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
