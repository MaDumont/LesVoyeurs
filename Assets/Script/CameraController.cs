using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	// Spacebar switch the camera pos/angle
	void Update () {
		if(Input.GetKeyDown (KeyCode.Space)){
			transform.Rotate(new Vector3(0,90,0));
		}
	}
}
