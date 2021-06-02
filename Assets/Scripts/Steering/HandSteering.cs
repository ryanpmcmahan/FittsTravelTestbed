using UnityEngine;

public class HandSteering : BaseLocomotion
{
    public bool speedChangeOnTilting;
    void Start()
    {
        Init();
    }

    // Update is called every graphical frame
    void Update()
    {
        activated = false;

        if (GetRightTouchPadPress())
        {
            Vector2 touchPadAxis = new Vector2();

            touchPadAxis = GetRightTouchPadAxis();

            activated = true;

            // No speed change on tilting
            Vector3 noSpeedChangeNorm = (new Vector3(rightHandObj.transform.forward.x, 0.0f, rightHandObj.transform.forward.z)).normalized;

            // Speed change on tilting
            Vector3 speedChangeNorm = (new Vector3(rightHandObj.transform.forward.x, rightHandObj.transform.forward.y, rightHandObj.transform.forward.z)).normalized;

            Vector3 horizontalDirectionNorm = speedChangeOnTilting ? new Vector3(speedChangeNorm.x, 0.0f, speedChangeNorm.z) : noSpeedChangeNorm;

            // Going forward
            if (touchPadAxis.y >= 0.0f)
            {
                user.transform.position += GetSpeed() * horizontalDirectionNorm * Time.deltaTime;
            }
            // Going backward
            else
            {
                user.transform.position -= GetSpeed() * horizontalDirectionNorm * Time.deltaTime;
            }

        }

        if (GetLeftTouchPadPress())
        {
            Vector2 touchPadAxis = new Vector2();

            touchPadAxis = GetLeftTouchPadAxis();

            activated = true;

            // No speed change on tilting
            Vector3 noSpeedChangeNorm = (new Vector3(leftHandObj.transform.forward.x, 0.0f, leftHandObj.transform.forward.z)).normalized;

            // Speed change on tilting
            Vector3 speedChangeNorm = (new Vector3(leftHandObj.transform.forward.x, leftHandObj.transform.forward.y, leftHandObj.transform.forward.z)).normalized;

            Vector3 horizontalDirectionNorm = speedChangeOnTilting ? new Vector3(speedChangeNorm.x, 0.0f, speedChangeNorm.z) : noSpeedChangeNorm;

            // Going forward
            if (touchPadAxis.y >= 0.0f)
            {
                user.transform.position += GetSpeed() * horizontalDirectionNorm * Time.deltaTime;
            }
            // Going backward
            else
            {
                user.transform.position -= GetSpeed() * horizontalDirectionNorm * Time.deltaTime;
            }
        }


    }

}
