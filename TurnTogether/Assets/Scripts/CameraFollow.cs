using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Drag your car/player here
    public Vector3 offset = new Vector3(0, 8, -6); // Camera position relative to player
    public float followSpeed = 5f; // Smoothness

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Optionally face the car
        transform.LookAt(target);
    }
}
