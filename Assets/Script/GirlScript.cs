using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GirlScript : MonoBehaviour {

	public Transform point;
	private NavMeshAgent agent;
	FOV2DEyes eyes;
	FOV2DVisionCone visionCone;
	Animator anim;
	Vector3 lastSeen;
	bool chasePlayer;
	
	enum State {IDLE, WALK, ALERT};
	State currentState;
	
	void Start () {
		GameManager.getInstance ().addGuardListener (this);
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
		agent.destination = point.position;
		anim.SetBool ("Move", true);

		InvokeRepeating ("CheckVision", 0, 0.3f);
	}
	
	public void gameStateChanged(GameState gameState)
	{
		if (gameState == GameState.Suspected)
			eyes.fovMaxDistance++;
		else if (gameState == GameState.Detected)
			agent.speed = 2;
	}
	
	void Idle(){
		chasePlayer = false;
		currentState = State.IDLE;
		anim.SetBool ("Move", false);
		agent.Stop();
	}
	
	void Alert(){
		visionCone.status = FOV2DVisionCone.Status.Alert;	
		if (currentState != State.ALERT && !chasePlayer) {
			GameManager.getInstance().updatePoints(-10);
			Debug.Log ("Alert!");
			currentState = State.ALERT;
			anim.SetBool ("Move", true);
			anim.SetBool ("Detection", true);
			agent.Stop ();
			
			GameManager.getInstance().stepUpGameState();
			GameManager.getInstance().setPlayerPos(this.transform.position);
		}
	}
	
	void NoAlert(){
		visionCone.status = FOV2DVisionCone.Status.Idle;
		
		if (currentState == State.ALERT) {
			Debug.Log ("NoAlert");
			anim.SetBool("Detection", false);
			Walk ();
		}
	}
	
	void Walk(){
		currentState = State.WALK;
		anim.SetBool ("Move", true);
		agent.Resume ();
		GotoNextPoint ();
	}

	void PointingDone()
	{
		anim.SetBool ("Move", true);
		Walk ();
	}

	public void heardNoise(Vector3 noisePos)
	{
		lastSeen = noisePos;
		chasePlayer = true;
		anim.SetBool ("Detection", false);
		Walk ();
	}
	
	void GotoNextPoint() {
		
		// Returns if no points have been set up
		if (agent.remainingDistance < 0.5f && !chasePlayer)
			return;
		
		if (chasePlayer) {
			agent.destination = lastSeen;
		}
		else {
			// Set the agent to go to the currently selected destination.
			agent.destination = point.position;
		}
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
				lastSeen = hit.collider.gameObject.transform.position;
			}
		}
		
		if (alert) {
			Alert ();
		}
		else
			NoAlert();
		chasePlayer = alert;
	}
}
