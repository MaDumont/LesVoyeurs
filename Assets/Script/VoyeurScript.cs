using UnityEngine;
using System.Collections;

public class VoyeurScript : MonoBehaviour {

	public float Speed;
	public float TurnSpeed;
	FOV2DEyes eyes;
	FOV2DVisionCone visionCone;
	Animator anim;
	bool targetAcquire = false;

	private Rigidbody _rigidBody;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		_rigidBody = GetComponent<Rigidbody> ();
		eyes = GetComponentInChildren<FOV2DEyes>();
		visionCone = GetComponentInChildren<FOV2DVisionCone>();

		InvokeRepeating ("CheckVision", 0, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
		float forwardVel = Input.GetAxis ("Horizontal");
		float sideVel = Input.GetAxis ("Vertical");

		anim.SetBool ("move", forwardVel != 0 || sideVel != 0);
		anim.SetBool ("sneak", Input.GetKey (KeyCode.LeftShift));

		float rotation = (Input.GetKey (KeyCode.Q) ? -1 : 0) + (Input.GetKey (KeyCode.E) ? 1 : 0);
		Vector3 velocity = new Vector3 (forwardVel, 0, sideVel) * Speed;
		velocity = transform.TransformDirection (velocity);
		_rigidBody.velocity = velocity;

		//_rigidBody.transform.Translate(velocity * Speed * Time.deltaTime);
		_rigidBody.transform.Rotate (new Vector3(0, rotation,0), TurnSpeed * Time.deltaTime);

		if (targetAcquire && Input.GetMouseButton (0)) {
			visionCone.status = FOV2DVisionCone.Status.Suspicious;
			anim.SetBool("camera", true);
		} else if (targetAcquire) {
			if(visionCone.status == FOV2DVisionCone.Status.Suspicious)
			{
				TakePicture();
			}
			anim.SetBool("camera", false);
			visionCone.status = FOV2DVisionCone.Status.Alert;
		}else{
			anim.SetBool("camera", false);
			visionCone.status = FOV2DVisionCone.Status.Idle;
		}


	}

	void CheckVision(){
	 	targetAcquire = false;
		CalculatePoints (false);
	}

	void TakePicture()
	{
		CalculatePoints (true);
	}

	void CalculatePoints(bool isPhoto)
	{
		ArrayList targetHits = new ArrayList ();
		foreach (RaycastHit hit in eyes.hits) 
		{
			if (hit.transform && hit.transform.tag == "Girl" && !targetHits.Contains(hit.transform.gameObject.GetHashCode())) 
			{
				targetAcquire = true;
				targetHits.Add(hit.transform.gameObject.GetHashCode());
				int points = (int)(Mathf.Min(10, 20 * Mathf.Max(0f, 1 - hit.distance/eyes.fovMaxDistance)));
				if(isPhoto)
				{
					points *= 100;
				}
				Debug.Log(points.ToString() + " points");
				GameManager.getInstance().updatePoints(points);
			}
		}
	}
}
