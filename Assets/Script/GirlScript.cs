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
		anim = GetComponent<Animator> ();
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
		
		InvokeRepeating ("CheckVision", 0, 0.3f);
	}

	public void gameStateChanged(GameState gameState)
	{
		if (gameState == GameState.Suspected) {
			eyes.fovMaxDistance++;
		}
		else if (gameState == GameState.Detected)
			agent.speed = 2;
	}
	
	void Idle(){
		currentState = State.IDLE;
		chasePlayer = false;
		anim.SetBool ("Move", false);
		anim.SetBool ("Detection", false);
		agent.Stop();
		if (agent.destination != point.position)
			GotoNextPoint ();
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
		}
		Walk ();
		GameManager.getInstance ().setPlayerPos (lastSeen);
	}
	
	void NoAlert(){
		visionCone.status = FOV2DVisionCone.Status.Idle;
		
		if (currentState == State.ALERT) {
			Debug.Log ("NoAlert");
			currentState = State.IDLE;
			anim.SetBool("", false);
			Walk ();
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Walk")) {
			chasePlayer = false;
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
		anim.SetBool ("Detection", false);
		chasePlayer = true;
		agent.speed = Mathf.Max(1, agent.speed);
		Walk ();
	}

	public void heardNoise(Vector3 noisePos)
	{
		lastSeen = noisePos;
		chasePlayer = true;
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
		if (chasePlayer)
			GameManager.getInstance().TryToCatchPlayer(this.transform.position);
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
			chasePlayer = true;
		} else {
			NoAlert ();
		}

	}
}