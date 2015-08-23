using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GirlScript : MonoBehaviour {

    FOV2DEyes eyes;
    FOV2DVisionCone visionCone;
    Animator anim;

    enum State { IDLE, WALK, ALERT };
    State currentState;

    private SoundManager soundManager;

    void Start()
    {
        GameManager.getInstance().addGuardListener(this);
        anim = GetComponentInChildren<Animator>();
        eyes = GetComponentInChildren<FOV2DEyes>();
        visionCone = GetComponentInChildren<FOV2DVisionCone>();
        currentState = State.IDLE;

        InvokeRepeating("CheckVision", 0, 0.3f);

        soundManager = SoundManager.getInstance();
    }

    public void gameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Suspected)
            eyes.fovMaxDistance++;
    }

    void Idle()
    {
        currentState = State.IDLE;
        anim.SetBool("Move", false);
    }

    void Alert()
    {
        visionCone.status = FOV2DVisionCone.Status.Alert;
        if (currentState != State.ALERT)
        {
            GameManager.getInstance().updatePoints(-10);
            Debug.Log("Alert!");
            currentState = State.ALERT;
            //anim.SetBool("Move", true);
            //anim.SetBool("Detect", true);

            GameManager.getInstance().stepUpGameState();

            soundManager.StartGirlScream();
        }
    }

    void NoAlert()
    {
        visionCone.status = FOV2DVisionCone.Status.Idle;

        if (currentState == State.ALERT)
        {
            Debug.Log("NoAlert");
            currentState = State.IDLE;
        }
    }

    void Walk()
    {
        currentState = State.WALK;

    }

    public void UhOhDone()
    {
        Walk();
    }

    public void IdleEnd()
    {
        if (currentState == State.IDLE)
            Walk();
    }

    public void heardNoise(Vector3 noisePos)
    {
        Walk();
    }


    void Update()
    {
    }

    void CheckVision()
    {
        bool alert = false;
        foreach (RaycastHit hit in eyes.hits)
        {
            if (hit.transform && hit.transform.tag == "Player")
            {
                alert = true;
            }
        }

        if (alert)
            Alert();
        else
            NoAlert();

    }
}
