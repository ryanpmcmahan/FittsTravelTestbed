using UnityEngine;

public class AnimatedTeleport : BaseLocomotion
{
    private VRTeleporter teleporter;

    private bool isMoving;

    private Vector3 startPos, endPos;
    private float startTime;

    void Start()
    {
        Init();
        teleporter = user.transform.GetComponentInChildren<VRTeleporter>();
        teleporter.ToggleDisplay(false);
        isMoving = false;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        startTime = 0.0f;
    }

    // Update is called every graphical frame
    void Update()
    {
        activated = false;

        if (GetRightTouchPadPressDown())
        {
            activated = true;
            GameObject.Find("TeleporterController").transform.parent = rightHandObj.transform;
            GameObject.Find("TeleporterController").transform.localPosition = Vector3.zero;
            GameObject.Find("TeleporterController").transform.localRotation = Quaternion.identity;
            teleporter.ToggleDisplay(true);
        }

        if (GetRightTouchPadPressUp())
        {
            startPos = user.transform.position;
            endPos = teleporter.AnimatedTeleport();
            startTime = Time.time;
            isMoving = true;
            teleporter.ToggleDisplay(false);
            GameObject.Find("TeleporterController").transform.parent = user.transform;
            GameObject.Find("TeleporterController").transform.localPosition = Vector3.zero;
            GameObject.Find("TeleporterController").transform.localRotation = Quaternion.identity;
        }

        if (GetLeftTouchPadPressDown())
        {
            activated = true;
            GameObject.Find("TeleporterController").transform.parent = leftHandObj.transform;
            GameObject.Find("TeleporterController").transform.localPosition = Vector3.zero;
            GameObject.Find("TeleporterController").transform.localRotation = Quaternion.identity;
            teleporter.ToggleDisplay(true);
        }

        if (GetLeftTouchPadPressUp())
        {
            startPos = user.transform.position;
            endPos = teleporter.AnimatedTeleport();
            startTime = Time.time;
            isMoving = true;
            teleporter.ToggleDisplay(false);
            GameObject.Find("TeleporterController").transform.parent = user.transform;
            GameObject.Find("TeleporterController").transform.localPosition = Vector3.zero;
            GameObject.Find("TeleporterController").transform.localRotation = Quaternion.identity;
        }

        if (isMoving)
        {
            activated = true;
            float distCovered = (Time.time - startTime) * GetSpeed();
            Vector3 updatedEndPos = endPos - new Vector3(Camera.main.transform.localPosition.x, 0.0f, Camera.main.transform.localPosition.z);
            float percentage = distCovered / Vector3.Distance(startPos, updatedEndPos);
            user.transform.position = Vector3.Lerp(startPos, updatedEndPos, percentage);
            if (percentage >= 1.0)
            {
                isMoving = false;
                activated = false;
                startTime = 0.0f;
                startPos = Vector3.zero;
                endPos = Vector3.zero;
            }
        }
    }

}
