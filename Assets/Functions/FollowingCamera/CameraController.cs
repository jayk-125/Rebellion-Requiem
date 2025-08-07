/* 
 * Author: Loh Shau Ern Shaun
 * Date: 14/5/2025
 * Camera follows the player camera
 * Stops following at the border of a room
 * Camera follows player to different rooms
 * Camera adapts to the room size
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Target to focus on
    public Transform target;
    // The difference from player and camera (so it doesnt become 1st person)
    public Vector3 offset;

    // Reference the relative camera middle
    public GameObject camMiddle;
    // Reference to camera limits for room border
    private Vector3 minPosition;
    private Vector3 maxPosition;

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
                float newYPos = transform.position.y;
                float newZPos = transform.position.z;

                // Check if at horizontal limit
                if (BorderCheck(targetPosition,"hori"))
                {
                    //Debug.Log("Hori Ok!~");
                    // Allow camera to move to the target position horizontally
                    newXPos = targetPosition.x;
                }
                // Check if at vertical limit
                if (BorderCheck(targetPosition, "vert"))
                {
                    //Debug.Log("Vert Ok!~");
                    // Allow camera to move to the target position vertically
                    newZPos = targetPosition.z;
                }

                // Allow camera to move to target position based on height
                newYPos = targetPosition.y;

                // Move camera
                transform.position = new Vector3(newXPos, newYPos, newZPos);
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
        //Debug.Log(relativeStart);

        // Get the current room's min world position
        Transform dynamicMinPos = totalObject.transform.Find("PositionHolder").Find("MinPosition");
        //Debug.Log(dynamicMinPos.position);
        // Get the current room's max world position
        Transform dynamicMaxPos = totalObject.transform.Find("PositionHolder").Find("MaxPosition");
        //Debug.Log(dynamicMaxPos.position);

        // Set relative min position
        minPosition.x = dynamicMinPos.position.x + 8f;
        minPosition.z = dynamicMinPos.position.z +1f;
        // Set relative max position
        maxPosition.x = dynamicMaxPos.position.x - 8f;
        maxPosition.z = dynamicMaxPos.position.z - 12f;

        // If it's not the starting room
        if (!isStart)
        {
            // Get the tele start object
            Transform teleStart = totalObject.transform.Find("TP_Exit");
            // Move camera to new position
            MoveCamera(teleStart.position);
        }
    }

    // Called when a teleport within a room is done
    public void MoveCamera(Vector3 newPoint)
    {
        // Set new estimated camera tp point based on offset
        newPoint.x += offset.x;
        newPoint.y += offset.y;
        newPoint.z += offset.z;

        // Check if it goes over the current borders for x
        if (newPoint.x > maxPosition.x)
        {
            newPoint.x = maxPosition.x;
        }
        else if (newPoint.x < minPosition.x)
        {
            newPoint.x = minPosition.x;
        }

        // Check if it goes over the current borders for z
        if (newPoint.z > maxPosition.z)
        {
            newPoint.z = maxPosition.z;
        }
        else if (newPoint.z < minPosition.z)
        {
            newPoint.z = minPosition.z;
        }

        // Move player to this new camera tp point
        gameObject.transform.position = newPoint;
    }
}
