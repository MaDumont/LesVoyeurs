using UnityEngine;
using System.Collections;

public class SoundManager: MonoBehaviour{

    private static SoundManager instance = null;
    private uint bankID;
    private SoundGaugeManager soundGaugeManager;

    public static SoundManager getInstance()
    {
        return instance;
    }

    public void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        AkSoundEngine.LoadBank("vjsoundbank", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
    }

    void Start()
    {
        soundGaugeManager = SoundGaugeManager.getInstance();
    }

    private void PlayEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    private void StopEvent(string eventName)
    {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 500, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }

    private void StopEvent(string eventName, GameObject soundController)
    {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, soundController, 500, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }


    public void StartGirlScream()
    {
        PlayEvent("Enfants_Surpris");
        soundGaugeManager.AddNoise(25);
    }
    public void StopGirlScream()
    {
        StopEvent("Enfants_Surpris");
    }
    public void StartParentScream()
    {
        PlayEvent("Homme_Hey");
        soundGaugeManager.AddNoise(50);
    }
    public void StopParentScream()
    {
        StopEvent("Homme_Hey");
    }
    public void StartStealthMusic()
    {
        PlayEvent("PlayMusic");
    }
    public void StopStealthMusic()
    {
        StopEvent("PlayMusic");
    }
    public void StartStressingMusic()
    {
        PlayEvent("PlayMusic");
    }
    public void StopStressingMussic()
    {
        StopEvent("PlayMusic");
    }
    public void StartIntroMusic()
    {
        PlayEvent("PlayMusic");
    }
    public void StopIntroMusic(GameObject musicController)
    {
        //je comprend pas pourquoi il faut lui donner le GameObject qui a parti la chanson
        StopEvent("PlayMusic", musicController);
    }
    public void StartWalkSound()
    {
        PlayEvent("Footsteps");
    }
    public void StopWalkSound()
    {
        StopEvent("Footsteps");
    }
    public void StartChienJappe()
    {
        PlayEvent("Chien_Jappe");
    }
    public void StopChienJappe()
    {
        StopEvent("Chien_Jappe");
    }
    public void StartWinLevelSound()
    {
        PlayEvent("LevelComplete");
    }
    public void StopWinLevelSound()
    {
        StopEvent("LevelComplete");
    }
    public void StartOpenDoorSound()
    {
        PlayEvent("Porte_Ouvre");
        soundGaugeManager.AddNoise(10);
    }
    public void StartCloseDoorSound()
    {
        PlayEvent("Porte_Ferme");
    }
    public void StartFloorCrackSound()
    {
        PlayEvent("Planche_craque");
        soundGaugeManager.AddNoise(20);
    }
    public void StartCameraSound()
    {
        PlayEvent("Camera");
        soundGaugeManager.AddNoise(50);
    }
    public void StartRespirationProfonde()
    {
        PlayEvent("Respiration_Profonde");
    }
    public void StartTelevisionSound()
    {
        PlayEvent("Television_Noise");
        soundGaugeManager.AddNoise(25);
    }
}
