using UnityEngine;
using System.Collections;

public class SoundManager: MonoBehaviour{

    uint bankID;

    public void Awake()
    {
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
        PlayEvent("Porte_Fermer");
    }
}
