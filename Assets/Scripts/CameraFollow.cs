using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Assign the player's transform
    public Vector3 offset = new Vector3(0, 0, -10);  // Default Z for 2D camera
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
