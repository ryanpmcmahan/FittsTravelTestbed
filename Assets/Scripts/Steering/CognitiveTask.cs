using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CUE_STATE { DISPLAY, HIDE };

public class CognitiveTask : MonoBehaviour
{
    public float cueDisplayTimer;
    public int cueHideMinSeed;
    public int cueHideMaxSeed;
    public AudioClip triggerPress;

    private CUE_STATE cueState;
    private TravelTask travelTask;
    private CueManager cueManager;
    private DataManager dataManager;
    private AudioSource audioSource;

    private float cueDisplayCountdown;
    private float cueHideCountdown;

    private bool buttonPress;

    private List<string> cueAndResponseList; // Cue, Cue Appear Time, Travel Phase, Response, Response Time, Travel Phase
    private int cueId;

    // Use this for initialization
    void Start()
    {
        travelTask = GetComponent<TravelTask>();
        cueManager = GetComponent<CueManager>();
        dataManager = GetComponent<DataManager>();
        audioSource = GetComponent<AudioSource>();

        cueState = CUE_STATE.HIDE;
        cueDisplayCountdown = 0.0f;
        cueHideCountdown = GetHideCountdown();

        buttonPress = false;

        // Data
        dataManager.cueAndResponse.Clear();
        cueAndResponseList = new List<string>();
        cueAndResponseList.Clear();

        cueId = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!travelTask.isStart || !travelTask.isStartCount)
            return;

        if (!buttonPress && cueId >= 0)
        {
            buttonPress = DetectButtonPress();
            if (buttonPress)
            {
                audioSource.PlayOneShot(triggerPress);
                cueAndResponseList.Add(buttonPress.ToString());
                cueAndResponseList.Add(Time.time.ToString());
                cueAndResponseList.Add(travelTask.travelPhase.ToString());
            }
        }

        switch (cueState)
        {
            case CUE_STATE.DISPLAY:

                cueDisplayCountdown -= Time.deltaTime;

                if (cueDisplayCountdown < 0.0f)
                {
                    cueDisplayCountdown = 0.0f;
                    cueState = CUE_STATE.HIDE;
                    cueManager.ShowCue(false);
                    cueHideCountdown = GetHideCountdown();
                }

                break;

            case CUE_STATE.HIDE:

                cueHideCountdown -= Time.deltaTime;

                if (cueHideCountdown < 0.0f)
                {
                    //Data
                    if (cueId >= 0)
                    {
                        dataManager.cueAndResponse.Add(cueId, cueAndResponseList);
                    }
                    cueAndResponseList = new List<string>();
                    buttonPress = false;

                    // Task
                    cueHideCountdown = 0.0f;
                    cueState = CUE_STATE.DISPLAY;
                    cueDisplayCountdown = cueDisplayTimer;
                    string tempCue = cueManager.GetCue();
                    cueManager.ShowCue(true);
                    cueId++;

                    // Data
                    cueAndResponseList.Add(tempCue);
                    cueAndResponseList.Add(Time.time.ToString());
                    cueAndResponseList.Add(travelTask.travelPhase.ToString());
                }
                break;

            default:
                break;
        }
    }

    private float GetHideCountdown()
    {
        System.Random r = new System.Random();
        return r.Next(cueHideMinSeed, cueHideMaxSeed) / 1000.0f;
    }

    private bool DetectButtonPress()
    {
        int rightIdx = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        int leftIdx = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);

        if (rightIdx != -1 && SteamVR_Controller.Input(rightIdx).GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            return true;
        }

        if (leftIdx != -1 && SteamVR_Controller.Input(leftIdx).GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            return true;
        }

        return false;
    }
}
