using UnityEngine;
using System.Collections;

public class SoundManager : AudioMaster{

    uint bankID;

    public void Start()
    {
        AkSoundEngine.LoadBank("VOICI VOILA", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
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


    //Il faut changer le nom des events !!!!!!!
    public void StartGirlScream()
    {
        PlayEvent("Voici VOila");
    }
    public void StopGirlScream()
    {
        StopEvent("Voici voila");
    }
    public void StartParentScream()
    {
        PlayEvent("Voici VOila");
    }
    public void StopParentScream()
    {
        StopEvent("Voici voila");
    }
    public void StartStealthMusic()
    {
        PlayEvent("Voici VOila");
    }
    public void StopStealthMusic()
    {
        StopEvent("Voici voila");
    }
    public void StartStressingMusic()
    {
        PlayEvent("Voici VOila");
    }
    public void StopStressingMussic()
    {
        StopEvent("Voici voila");
    }
    public void StartIntroMusic()
    {
        PlayEvent("Intro");
    }
    public void StopIntroMusic()
    {
        StopEvent("BDS");
    }
}
