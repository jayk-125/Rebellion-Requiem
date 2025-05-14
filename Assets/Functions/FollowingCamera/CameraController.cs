using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Target to focus on
    public Transform target;
    // Camera following smoothness (the slide toward the end of a follow)
    public float smoothing = 0f;
    // The difference from player and camera (so it doesnt become 1st person)
    public Vector3 offset;
    // Speed of camera following
    private float velocity = 100f;

    // Reference to camera limits for room border
    // Reference the relative camera middle
    public GameObject camMiddle;
    public Vector3 minPosition;
    public Vector3 maxPosition;

    // Awake is called when scene is started
    void Awake()
    {
        // Set the camera's borders
        SetCameraBorders(camMiddle,true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // If target has been assigned
        if (target != null)
        {
            // If the camera is not currently at the position of the player
            if ((transform.position + offset) != (target.position + offset))
            {
                // Set the current target position as the target + the offset
                Vector3 targetPosition = target.position + offset;
                //Debug.Log(targetPosition);

                // Init the position value variables
                float newXPos = transform.position.x;
                float newZPos = transform.position.z;

                // Check if at horizontal limit
                if (BorderCheck(targetPosition,"hori"))
                {
                    //Debug.Log("Hori Ok!~");
                    // Allow camera to move to the target position horizontally
                    newXPos = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity, smoothing);
                }
                // Check if at vertical limit
                if (BorderCheck(targetPosition, "vert"))
                {
                    //Debug.Log("Vert Ok!~");
                    // Allow camera to move to the target position vertically
                    newZPos = Mathf.SmoothDamp(transform.position.z, targetPosition.z, ref velocity, smoothing);
                }

                // Move camera
                transform.position = new Vector3(newXPos, transform.position.y, newZPos);
            }
        }
    }

    // Check either vertical or horizontal limit
    private bool BorderCheck(Vector3 checkPos,string checktype)
    {
        // Bool to confirm if valid target
        bool valid = false;
        
        // For hori check type
        if (checktype == "hori")
        {
            // Init x limits
            float limitMinX = minPosition.x;
            float limitMaxX = maxPosition.x;
            // If within x limit
            if ((checkPos.x >= limitMinX) && (checkPos.x <= limitMaxX))
            {
                // Set as valid
                //Debug.Log("Hori ok");
                valid = true;
            }
        }
        // If vert check type
        else if (checktype == "vert")
        {
            // Init z limits
            float limitMinZ = minPosition.z;
            float limitMaxZ = maxPosition.z;
            // If within z limit
            if ((checkPos.z >= limitMinZ) && (checkPos.z <= limitMaxZ))
            {
                // Set as valid
                //Debug.Log("Vert ok");
                valid = true;
            }
        }

        // Return bool result
        return valid;
    }

    // Set the camera's borders
    public void SetCameraBorders(GameObject totalObject, bool isStart)
    {
        // Get the cam middle object
        Transform relativeStart = totalObject.transform.Find("CamMiddle");
        Debug.Log(relativeStart);

        // Set min position
        minPosition.x = relativeStart.position.x - 6f;
        minPosition.z = relativeStart.position.z - 9.5f;
        // Set max position
        maxPosition.x = relativeStart.position.x + 6f;
        maxPosition.z = relativeStart.position.z + 8.5f;

        if (!isStart)
        {
            // Get the tele start object
            Transform teleStart = totalObject.transform.Find("TP_Exit");
            // Move camera to new position
            transform.position = new Vector3(teleStart.position.x, transform.position.y, teleStart.position.z);
        }
    }
}
