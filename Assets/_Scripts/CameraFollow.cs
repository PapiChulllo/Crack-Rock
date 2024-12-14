using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset;   // Offset to maintain from the player
    public float smoothSpeed = 0.125f; // Adjust for smooth following

    private void LateUpdate()
    {
        if (player == null) return;

        // Desired camera position based on the player's position and offset
        Vector3 desiredPosition = player.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera position
        transform.position = smoothedPosition;
    }
}
