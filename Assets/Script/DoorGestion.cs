using UnityEngine;
using System.Collections;

public class DoorGestion : MonoBehaviour {

    private Rigidbody _rigidbody;
    private bool doorIsOpen;
    private SoundManager soundManager;

	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        doorIsOpen = false;
        soundManager = SoundManager.getInstance();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            if (doorIsOpen) {
                CloseDoor();
            }
            else
            {
                OpenDoor();                
            }            
        }

    }

    public void OpenDoor()
    {
        _rigidbody.transform.Rotate(new Vector3(0, _rigidbody.transform.position.y, 0), -90);
        doorIsOpen = true;
        soundManager.StartOpenDoorSound();
    }

    public void CloseDoor()
    {
        _rigidbody.transform.Rotate(new Vector3(0, _rigidbody.transform.position.y, 0), 90);
        doorIsOpen = false;
        soundManager.StartCloseDoorSound();
    }


}
