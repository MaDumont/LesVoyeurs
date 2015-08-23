using UnityEngine;
using System.Collections;
using AssemblyCSharp;
public class GuardScript : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private NavMeshAgent agent;
	FOV2DEyes eyes;
	FOV2DVisionCone visionCone;
	Animator anim;
	Vector3 lastSeen;
	bool chasePlayer;

	enum State {IDLE, WALK, ALERT};
	State currentState;

    private SoundManager soundManager;
	
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
		agent.destination = points[destPoint].position;

		InvokeRepeating ("CheckVision", 0, 0.3f);

        soundManager = SoundManager.getInstance();
	}

	public void gameStateChanged(GameState gameState)
	{
		if (gameState == GameState.Suspected)
			eyes.fovMaxDistance++;
		else if (gameState == GameState.Detected)
			agent.speed *= 1.25f;
	}

	void Idle(){
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
			anim.SetBool ("Detect", true);
			agent.Stop ();

			GameManager.getInstance().stepUpGameState();

            soundManager.StartParentScream();
            soundManager.StartChienJappe();
		}
	}
			
	void NoAlert(){
		visionCone.status = FOV2DVisionCone.Status.Idle;

		if (currentState == State.ALERT) {
			Debug.Log ("NoAlert");
			currentState = State.IDLE;
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

	public void UhOhDone()
	{
		anim.SetBool ("Detect", false);
		Walk ();
	}
	
	public void IdleEnd(){
		if(currentState == State.IDLE)
			Walk ();
	}

	public void heardNoise(Vector3 noisePos)
	{
		lastSeen = noisePos;
		chasePlayer = true;
		anim.SetBool ("Detect", false);
		Walk ();
	}

	void GotoNextPoint() {

		// Returns if no points have been set up
		if (points.Length == 0 && !chasePlayer)
			return;

		if (chasePlayer) {
			agent.destination = lastSeen;
		}
		else {
			// Set the agent to go to the currently selected destination.
			agent.destination = points [destPoint].position;
		
			// Choose the next point in the array as the destination,
			// cycling to the start if necessary.
			destPoint = (destPoint + 1) % points.Length;
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

		if(alert)
			Alert();
		else
			NoAlert();

		
		chasePlayer = alert;
	}
}