using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour {

    private SoundManager soundManager;

	// Use this for initialization
	void Start () {
        soundManager = SoundManager.getInstance();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            other.attachedRigidbody.AddForce(new Vector3(1000, 1000, 0));
            soundManager.StopIntroMusic();
            soundManager.StartWinLevelSound();
        }

    }

 

}
