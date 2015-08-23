﻿using UnityEngine;
using System.Collections;

public class SoundManager: MonoBehaviour{

    private static SoundManager instance = null;
    private uint bankID;

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

    private void PlayEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    private void StopEvent(string eventName)
    {
        uint eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 0, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }


    public void StartGirlScream()
    {
        PlayEvent("Enfants_Surpris");
    }
    public void StopGirlScream()
    {
        StopEvent("Enfants_Surpris");
    }
    public void StartParentScream()
    {
        PlayEvent("Homme_Hey");
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
    public void StopIntroMusic()
    {
        StopEvent("PLayMusic");
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
    }
    public void StartCloseDoorSound()
    {
        PlayEvent("Porte_Ferme");
    }
    public void StartFloorCrackSound()
    {
        PlayEvent("Planche_craque");
    }
}
