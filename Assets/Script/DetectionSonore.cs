using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetectionSonore : MonoBehaviour {


    private readonly float SOUND_BROADCASTING_RUN = 10;
    private readonly float SOUND_BROADCASTING_WALK = 5;
    private readonly float SOUND_BROADCASTING_CROUNCH = 2;
    private readonly float SOUND_BROADCASTING_STILL = 0;

    private readonly float QUANTITY_GIRL_HEAR_SOUND = 0.1f;
    private readonly float QUANTITY_PARENT_HEAR_SOUND = 1f;

    private readonly int MAX_SLIDER_VALUE = 100;

    public Slider sliderSound;
    

    private Rigidbody _rigidBody; 
    private float distanceEntendu;
    private float totalNoiseHear;
    private SoundManager soundManager;

	// Use this for initialization
	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        distanceEntendu = MAX_SLIDER_VALUE;
        sliderSound.maxValue = SOUND_BROADCASTING_RUN; 
        totalNoiseHear = 0;
        soundManager = SoundManager.getInstance();
	}
	
	// Update is called once per frame
	void Update () {
        if (_rigidBody.velocity.magnitude <= 0.1)
        {
            distanceEntendu = SOUND_BROADCASTING_STILL;
            soundManager.StopWalkSound();
        }
        else if ( _rigidBody.velocity.magnitude <= 3)
        {
            distanceEntendu = SOUND_BROADCASTING_CROUNCH;
            soundManager.StartWalkSound();
        }
        else if (_rigidBody.velocity.magnitude < 6.5)
        {
            distanceEntendu = SOUND_BROADCASTING_WALK;
            soundManager.StartWalkSound();
        }
        else if (_rigidBody.velocity.magnitude >= 6.5)
        {
            distanceEntendu = SOUND_BROADCASTING_RUN;
            soundManager.StartWalkSound();
        }

        sliderSound.value = distanceEntendu;	
	}

    void LateUpdate() {

        if (totalNoiseHear > 0)
        {
            totalNoiseHear -= totalNoiseHear / MAX_SLIDER_VALUE;
        }

        Collider[] hitColliders = Physics.OverlapSphere(_rigidBody.transform.position, distanceEntendu);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Girl"){
                totalNoiseHear += _rigidBody.velocity.magnitude *QUANTITY_GIRL_HEAR_SOUND / distanceBetweenPlayerAndObject(hitColliders[i]) ;
            }
            if (hitColliders[i].tag == "Guard")
            {
                totalNoiseHear += _rigidBody.velocity.magnitude * QUANTITY_PARENT_HEAR_SOUND / distanceBetweenPlayerAndObject(hitColliders[i]) ;
            }
        }

        sliderSound.value = totalNoiseHear;
    }

    private float distanceBetweenPlayerAndObject(Collider obj)
    {
        return Mathf.Abs(obj.transform.position.magnitude - _rigidBody.transform.position.magnitude);
    }

}
