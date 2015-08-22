using UnityEngine;
using System.Collections;

public class VoyeurScript : MonoBehaviour {

    public float Speed;
	public float TurnSpeed;
	FOV2DEyes eyes;
	FOV2DVisionCone visionCone;

	private Rigidbody _rigidBody;


	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody> ();
		eyes = GetComponentInChildren<FOV2DEyes>();
		visionCone = GetComponentInChildren<FOV2DVisionCone>();

		InvokeRepeating ("CheckVision", 0, 0.3f);
        
	}
	
	// Update is called once per frame
	void Update () {
		float forwardVel = Input.GetAxis ("Horizontal");
		float sideVel = Input.GetAxis ("Vertical");

		float rotation = (Input.GetKey (KeyCode.Q) ? -1 : 0) + (Input.GetKey (KeyCode.E) ? 1 : 0);
		Vector3 velocity = new Vector3 (forwardVel, 0, sideVel) * Speed;
		velocity = transform.TransformDirection (velocity);
		_rigidBody.velocity = velocity;

		//_rigidBody.transform.Translate(velocity * Speed * Time.deltaTime);
		_rigidBody.transform.Rotate (new Vector3(0, rotation,0), TurnSpeed * Time.deltaTime);
	}

	void CheckVision(){
		bool targetAcquire = false;
		foreach (RaycastHit hit in eyes.hits) {
			if (hit.transform && hit.transform.tag == "Girl") {
				targetAcquire = true;
			}
		}
		
		if (targetAcquire) {
			visionCone.status = FOV2DVisionCone.Status.Alert;
		} else {
			visionCone.status = FOV2DVisionCone.Status.Idle;
		}
	}

}
