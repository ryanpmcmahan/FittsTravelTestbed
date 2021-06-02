using System.Collections.Generic;
using UnityEngine;

public enum PHASE_TYPE { NOTTRAVEL, BALLISTIC, REFINEMENT, EXIT };

public class TravelTask : MonoBehaviour
{
    public bool isTraining;
    public float startCountdown;
    public float refinementCountdown;


    [HideInInspector]
    public bool isStart;
    [HideInInspector]
    public bool isStartCount;
    [HideInInspector]
    public PHASE_TYPE travelPhase;

    private int trainingTargetNum;

    private CueManager cueManager;
    private CylinderManager cylinderManager;
    private DataManager dataManager;

    private int[] travelTargets;
    private int currentTravelIndex;

    private float countdownValue;

    private float ballisticTime, refinementTime, exitTime;

    // Use this for initialization
    void Start()
    {
        trainingTargetNum = 6;

        cueManager = GetComponent<CueManager>();
        cylinderManager = GetComponent<CylinderManager>();
        dataManager = GetComponent<DataManager>();

        travelPhase = PHASE_TYPE.NOTTRAVEL;

        travelTargets = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        if (isTraining)
        {
            int[] temp = new int[trainingTargetNum];
            for (int i = 0; i < trainingTargetNum; i++)
            {
                temp[i] = travelTargets[i];
            }

            travelTargets = temp;
        }

        currentTravelIndex = 0;

        isStart = false;
        isStartCount = false;
        
        // Data
        dataManager.trackingPositions.Clear();
        dataManager.travelTimes.Clear();

        ballisticTime = 0.0f;
        refinementTime = 0.0f;
        exitTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isStart = true;
            countdownValue = startCountdown;
        }

        if (!isStart)
            return;
        else if (!isStartCount)
        {
            if (countdownValue >= 0.0f)
            {
                countdownValue -= Time.deltaTime;
                cueManager.ShowCountdown(true);
                cueManager.SetCountdownValue(countdownValue, countdownValue / startCountdown);
            }
            else
            {
                countdownValue = 0.0f;
                isStartCount = true;
                cueManager.ShowCountdown(false);
                travelPhase = PHASE_TYPE.BALLISTIC;
            }
        }
        else
        {
            if (currentTravelIndex >= travelTargets.Length)
            {
                // Data
                List<float> tempTimes = new List<float>();
                tempTimes.Add(ballisticTime);
                tempTimes.Add(refinementTime);
                tempTimes.Add(exitTime);
                dataManager.travelTimes.Add(currentTravelIndex - 1, tempTimes);

                QuitGame();
            }

            switch (travelPhase)
            {
                case PHASE_TYPE.BALLISTIC:
                    ballisticTime += Time.deltaTime;
                    if (CheckInsideCylinder(currentTravelIndex))
                    {
                        countdownValue = refinementCountdown;
                        travelPhase = PHASE_TYPE.REFINEMENT;
                    }
                    break;

                case PHASE_TYPE.REFINEMENT:
                    refinementTime += Time.deltaTime;
                    if (countdownValue >= 0.0f)
                    {
                        countdownValue -= Time.deltaTime;
                        cueManager.ShowCountdown(true);
                        cueManager.SetCountdownValue(countdownValue, countdownValue / refinementCountdown);

                        if (GetComponent<BaseLocomotion>().Activated)
                        {
                            countdownValue = refinementCountdown;
                        }

                        if (!CheckInsideCylinder(currentTravelIndex))
                        {
                            countdownValue = refinementCountdown;
                            cueManager.ShowCountdown(false);
                        }
                    }
                    else
                    {
                        currentTravelIndex++;
                        countdownValue = 0.0f;
                        cueManager.ShowCountdown(false);
                        travelPhase = PHASE_TYPE.EXIT;
                    }
                    break;

                case PHASE_TYPE.EXIT:
                    exitTime += Time.deltaTime;
                    if (!CheckInsideCylinder(currentTravelIndex - 1))
                    {
                        travelPhase = PHASE_TYPE.BALLISTIC;

                        // Data
                        // Times: 1. Ballistic Time 2. Refinement Time 3. Exit Time
                        List<float> tempTimes = new List<float>();
                        tempTimes.Add(ballisticTime);
                        tempTimes.Add(refinementTime);
                        tempTimes.Add(exitTime);
                        dataManager.travelTimes.Add(currentTravelIndex - 1, tempTimes);
                        ballisticTime = 0.0f;
                        refinementTime = 0.0f;
                        exitTime = 0.0f;
                    }
                    break;

                case PHASE_TYPE.NOTTRAVEL:
                default:
                    break;

            }

            UpdateTargetState();

            // Tracking Positions: 1. CameraRig Global 2. User Head Local 3. first Controller local 4. second Controller Local 5. first Tracker Local 6. second Tracker Local
            List<float> tempTrackerList = new List<float>();
            tempTrackerList.Add(FindObjectOfType<SteamVR_ControllerManager>().transform.position.x);
            tempTrackerList.Add(FindObjectOfType<SteamVR_ControllerManager>().transform.position.y);
            tempTrackerList.Add(FindObjectOfType<SteamVR_ControllerManager>().transform.position.z);
            tempTrackerList.Add(Camera.main.transform.localPosition.x);
            tempTrackerList.Add(Camera.main.transform.localPosition.y);
            tempTrackerList.Add(Camera.main.transform.localPosition.z);
            foreach (GameObject obj in FindObjectOfType<SteamVR_ControllerManager>().objects)
            {
                tempTrackerList.Add(obj.transform.localPosition.x);
                tempTrackerList.Add(obj.transform.localPosition.y);
                tempTrackerList.Add(obj.transform.localPosition.z);
            }
            dataManager.trackingPositions.Add(Time.time, tempTrackerList);
        }
    }

    private bool CheckInsideCylinder(int index)
    {
        if (index >= travelTargets.Length)
            return false;

        Vector2 headHPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);
        Vector3 cylinderPosition = cylinderManager.GetCylinderPosition(travelTargets[index]);
        Vector2 cylinderHPosition = new Vector2(cylinderPosition.x, cylinderPosition.z);

        float dis = Vector2.Distance(headHPosition, cylinderHPosition);

        if (dis <= (cylinderManager.cylinderDiameter / 2.0f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateTargetState()
    {
        for (int i = 0; i < travelTargets.Length; i++)
        {
            cylinderManager.SetCylinderState(travelTargets[i], CYLINDERSTATE.DEFAULT);

            if (currentTravelIndex == i)
            {
                cylinderManager.SetCylinderState(travelTargets[i], CYLINDERSTATE.TARGET);
            }

            if (CheckInsideCylinder(i) && currentTravelIndex == i)
            {
                cylinderManager.SetCylinderState(travelTargets[i], CYLINDERSTATE.INSIDE);
            }
        }

        cylinderManager.UpdateCylinderMaterial();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
