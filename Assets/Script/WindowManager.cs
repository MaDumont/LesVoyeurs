using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour {

    private SoundManager soundManager;
    private Rigidbody windowRigidboyd;
    public GameObject musicController;
    public GameObject weakWindow;

	// Use this for initialization
	void Start () {
        soundManager = SoundManager.getInstance();
        windowRigidboyd = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            DestroyWindow();
        }

    }

    private void DestroyWindow()
    {
        //yield return new WaitForSeconds(1);
        Instantiate(weakWindow, transform.position, transform.rotation);
        Destroy(gameObject);
        soundManager.StopIntroMusic(musicController);
        soundManager.StartWinLevelSound();
    }

 

}
