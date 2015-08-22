using UnityEngine;
using System.Collections;

public class AudioMaster : MonoBehaviour {

    uint bankID;
    // Use this for initialization
    protected void LoadBank()
    {
        AkSoundEngine.LoadBank("VOICI VOILA", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PLayEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    public void StopEvent(string eventName, int fadeout)
    {

        uint eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, fadeout * 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }
}
