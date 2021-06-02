using UnityEngine;

public class TorsoSteering : BaseLocomotion
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

        if (GetRightTouchPadPress() || GetLeftTouchPadPress())
        {
            Vector2 touchPadAxis = new Vector2();

            if (GetRightTouchPadPress())
                touchPadAxis = GetRightTouchPadAxis();
            else if (GetLeftTouchPadPress())
                touchPadAxis = GetLeftTouchPadAxis();

            activated = true;

            // No speed change on tilting
            Vector3 noSpeedChangeNorm = (new Vector3(torsoTrackerObj.transform.forward.x, 0.0f, torsoTrackerObj.transform.forward.z)).normalized;

            // Speed change on tilting
            Vector3 speedChangeNorm = (new Vector3(torsoTrackerObj.transform.forward.x, torsoTrackerObj.transform.forward.y, torsoTrackerObj.transform.forward.z)).normalized;

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
