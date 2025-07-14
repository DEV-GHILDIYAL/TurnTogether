using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No main camera found in the scene!");
        }
    }

    void LateUpdate()
    {
        // Check if the camera is assigned
        if (cam != null)
        {
            // Make the object face the camera
            transform.LookAt(transform.position + cam.transform.forward);
        }
    }
}
