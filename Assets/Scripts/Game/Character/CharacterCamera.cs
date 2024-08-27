using UnityEngine;

public class CameraFollowNoRotation : MonoBehaviour
{
    public Transform target; // The character to follow
    public Vector3 offset; // Offset from the target
    public Vector2 minCameraBounds; // Minimum camera bounds
    public Vector2 maxCameraBounds; // Maximum camera bounds

    private Vector3 targetPosition;

    void LateUpdate()
    {
        // Calculate the target position the camera should move to
        targetPosition = target.position + offset;

        // Clamp the target position within the camera's bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, minCameraBounds.x, maxCameraBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minCameraBounds.y, maxCameraBounds.y);

        // Apply the constrained position to the camera
        transform.position = targetPosition;

        // Keep the camera's rotation fixed
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
