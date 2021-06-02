using UnityEngine;

public class BaseLocomotion : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected GameObject user;
    protected GameObject leftHandObj;
    protected GameObject rightHandObj;
    protected GameObject torsoTrackerObj;
    protected Transform userHead;
    protected SteamVR_Controller.Device leftHand;
    protected SteamVR_Controller.Device rightHand;
    protected int leftHandId;
    protected int rightHandId;

    protected bool activated;

    public void Init()
    {
      // int leftHandId = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
      //  int rightHandId = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

        user = FindObjectOfType<SteamVR_ControllerManager>().gameObject;

        user.transform.position = Vector3.zero;
        userHead = Camera.main.transform;
        activated = false;

        leftHandObj = FindObjectOfType<SteamVR_ControllerManager>().left;
        rightHandObj = FindObjectOfType<SteamVR_ControllerManager>().right;

      //  leftHandObj.GetComponent<SteamVR_TrackedObject>().SetDeviceIndex(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));

       // rightHandObj.GetComponent<SteamVR_TrackedObject>().SetDeviceIndex(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost));

        leftHandId = (int) leftHandObj.GetComponent<SteamVR_TrackedObject>().index;
        rightHandId = (int) rightHandObj.GetComponent<SteamVR_TrackedObject>().index;

        if (FindObjectOfType<SteamVR_ControllerManager>().objects.Length == 3)
        {
            torsoTrackerObj = FindObjectOfType<SteamVR_ControllerManager>().objects[2];
        }

        if (leftHandId != -1)
            leftHand = SteamVR_Controller.Input(leftHandId);
        else          
            leftHand = null;

        if (rightHandId != -1)
            rightHand = SteamVR_Controller.Input(rightHandId);
        else
            rightHand = null;
    }

    public bool Activated
    {
        get
        {
            return activated;
        }
    }

    public virtual float GetSpeed()
    {
        return speed;
    }

    public bool GetLeftTriggerPress()
    {
        CheckId();

        if (leftHand != null)
            return leftHand.GetPress(SteamVR_Controller.ButtonMask.Trigger);

        return false;
    }

    public bool GetRightTriggerPress()
    {
        CheckId();

        if (rightHand != null)
            return rightHand.GetPress(SteamVR_Controller.ButtonMask.Trigger);

        return false;
    }

    public bool GetTriggerPress()
    {
        CheckId();

        if (GetLeftTriggerPress() || GetRightTriggerPress())
            return true;

        return false;
    }

    public bool GetLeftTouchPadPress()
    {
        CheckId();

        if (leftHand != null)
            return leftHand.GetPress(SteamVR_Controller.ButtonMask.Touchpad);

        return false;
    }

    public bool GetRightTouchPadPress()
    {
        CheckId();

        if (rightHand != null)
        {
            return rightHand.GetPress(SteamVR_Controller.ButtonMask.Touchpad);

        }
        return false;
    }

    public bool GetTouchPadPress()
    {
        CheckId();

        if (GetLeftTouchPadPress() || GetRightTouchPadPress())
            return true;

        return false;
    }

    public bool GetLeftTouchPadPressUp()
    {
        CheckId();

        if (leftHand != null)
            return leftHand.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);

        return false;
    }

    public bool GetRightTouchPadPressUp()
    {
        CheckId();

        if (rightHand != null)
            return rightHand.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);

        return false;
    }

    public bool GetTouchPadPressUp()
    {
        CheckId();

        if (GetLeftTouchPadPressUp() || GetRightTouchPadPressUp())
            return true;

        return false;
    }

    public bool GetLeftTouchPadPressDown()
    {
        CheckId();

        if (leftHand != null)
            return leftHand.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);

        return false;
    }

    public bool GetRightTouchPadPressDown()
    {
        CheckId();

        if (rightHand != null)
            return rightHand.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);

        return false;
    }

    public bool GetTouchPadPressDown()
    {
        CheckId();

        if (GetLeftTouchPadPressDown() || GetRightTouchPadPressDown())
            return true;

        return false;
    }

    public Vector2 GetLeftTouchPadAxis()
    {
        CheckId();

        if (leftHand != null)
            return leftHand.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

        return Vector2.zero;
    }

    public Vector2 GetRightTouchPadAxis()
    {
        CheckId();

        if (rightHand != null)
            return rightHand.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

        return Vector2.zero;
    }

    private void CheckId()
    {
        if (leftHandId == -1)
        {
            leftHandId = (int)leftHandObj.GetComponent<SteamVR_TrackedObject>().index;
            if (leftHandId != -1)
                leftHand = SteamVR_Controller.Input(leftHandId);
            else
                leftHand = null;
        }
        if (rightHandId == -1)
        {
            rightHandId = (int)rightHandObj.GetComponent<SteamVR_TrackedObject>().index;
            if (rightHandId != -1)
                rightHand = SteamVR_Controller.Input(rightHandId);
            else
                rightHand = null;
        }
    }
}
