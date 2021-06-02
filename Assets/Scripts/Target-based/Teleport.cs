using UnityEngine;

public class Teleport : BaseLocomotion
{
    private VRTeleporter teleporter;
  
    void Start()
    {
        Init();
        teleporter = user.transform.GetComponentInChildren<VRTeleporter>();
        teleporter.ToggleDisplay(false);
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
            activated = false;
            teleporter.Teleport();
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
            activated = false;
            teleporter.Teleport();
            teleporter.ToggleDisplay(false);
            GameObject.Find("TeleporterController").transform.parent = user.transform;
            GameObject.Find("TeleporterController").transform.localPosition = Vector3.zero;
            GameObject.Find("TeleporterController").transform.localRotation = Quaternion.identity;
        }
    }

}
