using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    /// <summary>
    /// X axis sensation of the camera.
    /// </summary>
    public float sensX;
    
    /// <summary>
    /// Y axis sensation of the camera.
    /// </summary>
    public float sensY;

    /// <summary>
    /// Orientation of the transform object of the camera.
    /// </summary>
    public Transform orientation;

    /// <summary>
    /// X axis rotation of the camera.
    /// </summary>
    public float xRotation;
    
    /// <summary>
    /// Y axis rotation of the camera.
    /// </summary>
    public float yRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;
        
        // calculate rotation
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        // apply rotation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
}
