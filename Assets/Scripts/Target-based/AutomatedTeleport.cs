using UnityEngine;
using System.Collections.Generic;

public class AutomatedTeleport : BaseLocomotion
{
    private Queue<Vector3> targetQueue;
    private Vector3 startPos, endPos;
    private float startTime;
    void Start()
    {
        Init();
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        startTime = 0.0f;

        targetQueue = new Queue<Vector3>();
        for (int i = 0; i < GetComponent<CylinderManager>().cylinderCount; i++)
        {
            targetQueue.Enqueue(new Vector3(GetComponent<CylinderManager>().GetCylinderPosition(i).x, 0.0f, GetComponent<CylinderManager>().GetCylinderPosition(i).z));
        }
    }

    // Update is called every graphical frame
    void Update()
    {
        activated = false;
        if (GetComponent<TravelTask>().isStart)
        {
            if (GetComponent<TravelTask>().autoNext)
            {
                if (UserLookAtTarget())
                {
                    activated = true;
                    GetComponent<TravelTask>().autoNext = false;

                    endPos = targetQueue.Dequeue();// - new Vector3(Camera.main.transform.localPosition.x, 0.0f, Camera.main.transform.localPosition.z);
                    startTime = Time.time;
                    startPos = user.transform.position;
                }
            }
            else
            {
                float distCovered = (Time.time - startTime) * GetSpeed();

                Vector3 updatedEndPos = endPos - new Vector3(Camera.main.transform.localPosition.x, 0.0f, Camera.main.transform.localPosition.z);

                float percentage = distCovered / Vector3.Distance(startPos, updatedEndPos);
                user.transform.position = Vector3.Lerp(startPos, updatedEndPos, percentage);
                
                if (percentage >= 1.0f)
                {
                    activated = false;
                }  
            }
        }

    }

    bool UserLookAtTarget()
    {
        Vector3 targetPos = targetQueue.Peek();
        Vector3 targetDir = targetPos - Camera.main.transform.position;
        Vector3 lookingDir = Camera.main.transform.forward;
        if (Mathf.Abs(Vector3.SignedAngle(targetDir, lookingDir, Vector3.up)) < 20.0f)
        {
            Debug.Log(Vector3.SignedAngle(targetDir, lookingDir, Vector3.up));
            return true;
        }

        return false;
    }

}
