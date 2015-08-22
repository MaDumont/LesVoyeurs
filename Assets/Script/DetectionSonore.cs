using UnityEngine;
using System.Collections;

public class DetectionSonore : MonoBehaviour {

    //nb de cube que les ennemis entendes 
    public int distanceEntendu;

    private Rigidbody player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Rigidbody>();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate() {
        Physics.OverlapSphere(player.transform.position, distanceEntendu);
    
    }
}
