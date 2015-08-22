using UnityEngine;
using System.Collections;

public class GuardScript : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	FOV2DEyes eyes;
	FOV2DVisionCone visionCone;
	Animator anim;

	enum State {IDLE, WALK, ALERT};
	State currentState;
	
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		agent = GetComponent<NavMeshAgent>();  
		eyes = GetComponentInChildren<FOV2DEyes>();
		visionCone = GetComponentInChildren<FOV2DVisionCone>();
		currentState = State.IDLE;
		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		agent.autoBraking = false;
		agent.Stop ();
		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		InvokeRepeating ("CheckVision", 0, 0.3f);
	}

	void Idle(){
		currentState = State.IDLE;
		anim.SetBool ("Move", false);
		agent.Stop();
	}

	void Alert(){
		if (currentState != State.ALERT) {
			Debug.Log ("Alert!");
			currentState = State.ALERT;
			visionCone.status = FOV2DVisionCone.Status.Alert;
			anim.SetBool ("Move", false);
			anim.SetBool ("Detect", true);
			agent.Stop ();
		}
	}

	void NoAlert(){
		if (currentState == State.ALERT) {
			Debug.Log ("NoAlert");
			currentState = State.IDLE;
			visionCone.status = FOV2DVisionCone.Status.Idle;
			anim.SetBool("Detect", false);
		}
	}

	void Walk(){
		currentState = State.WALK;
		anim.SetBool ("Move", true);
		anim.SetBool ("Detect", false);
		agent.Resume ();
		GotoNextPoint ();
	}

	public void IdleEnd(){
		if(currentState == State.IDLE)
			Walk ();
	}

	void GotoNextPoint() {

		// Returns if no points have been set up
		if (points.Length == 0)
			return;
		
		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;
		
		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}
	
	
	void Update () {
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (currentState == State.WALK && agent.remainingDistance < 0.5f) {
			Idle ();
		}
	}

	void CheckVision(){

		bool alert = false;
		foreach (RaycastHit hit in eyes.hits) {
			if (hit.transform && hit.transform.tag == "Player") {
				alert = true;
			}
		}

		if(alert)
			Alert();
		else
			NoAlert();
	}
}
