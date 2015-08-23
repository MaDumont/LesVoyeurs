using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundGaugeManager: MonoBehaviour{


    private readonly float SOUND_BROADCASTING_RUN = 10;
    private readonly float SOUND_BROADCASTING_WALK = 5;
    private readonly float SOUND_BROADCASTING_CROUNCH = 2;
    private readonly float SOUND_BROADCASTING_STILL = 0;

    private readonly float QUANTITY_GIRL_HEAR_SOUND = 0.05f;
    private readonly float QUANTITY_PARENT_HEAR_SOUND = 0.5f;

    private readonly int MAX_SLIDER_VALUE = 100;
    
    private Rigidbody playerRigidBody; 
    private float distanceEntendu;
    private float totalNoiseHear;

	// Use this for initialization
    void Awake()
    {
        distanceEntendu =SOUND_BROADCASTING_RUN ;
        totalNoiseHear = 0;
        playerRigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	public void Update () {
        if (playerRigidBody.velocity.magnitude <= 0.1)
        {
            distanceEntendu = SOUND_BROADCASTING_STILL;
        }
        else if ( playerRigidBody.velocity.magnitude <= 3)
        {
            distanceEntendu = SOUND_BROADCASTING_CROUNCH;
        }
        else if (playerRigidBody.velocity.magnitude < 6.5)
        {
            distanceEntendu = SOUND_BROADCASTING_WALK;
        }
        else if (playerRigidBody.velocity.magnitude >= 6.5)
        {
            distanceEntendu = SOUND_BROADCASTING_RUN;
        }
	}

    void LateUpdate() {

        if (totalNoiseHear > 0)
        {
            totalNoiseHear -= totalNoiseHear / MAX_SLIDER_VALUE;
        }

        Collider[] hitColliders = Physics.OverlapSphere(playerRigidBody.transform.position, distanceEntendu);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Girl"){
                totalNoiseHear += playerRigidBody.velocity.magnitude *QUANTITY_GIRL_HEAR_SOUND / distanceBetweenPlayerAndObject(hitColliders[i]) ;
            }
            if (hitColliders[i].tag == "Guard")
            {
                totalNoiseHear += playerRigidBody.velocity.magnitude * QUANTITY_PARENT_HEAR_SOUND / distanceBetweenPlayerAndObject(hitColliders[i]) ;
            }
        }
    }

    void OnGUI()
    {
        var x = Screen.width - 100;
        GUI.HorizontalSlider(new Rect(x, 0, 100, 100), totalNoiseHear, 0, MAX_SLIDER_VALUE);
    }

    private float distanceBetweenPlayerAndObject(Collider obj)
    {
        return Mathf.Abs(obj.transform.position.magnitude - playerRigidBody.transform.position.magnitude);
    }

}
