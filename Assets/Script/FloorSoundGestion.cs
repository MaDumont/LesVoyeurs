using UnityEngine;
using System.Collections;

public class FloorSoundGestion : MonoBehaviour {

    private SoundManager soundManager;


    // Use this for initialization
    void Start()
    {
        soundManager = SoundManager.getInstance();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            soundManager.StartFloorCrackSound();
        }

    }

}
