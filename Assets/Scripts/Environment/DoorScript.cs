using UnityEngine;
using Interfaces;

public class DoorScript : MonoBehaviour, IInteractable
{
    public bool isOpen = false;

    public Transform doorTransform;
    public float rotationSpeed = 20f; // Speed of rotation
    public Vector3 openRotation = new Vector3(0, 0, 0); // Rotation when the door is open
    public Vector3 closedRotation = new Vector3(0, 0, 0); // Rotation when the door is closed
    public KeyCode InteractionKey => KeyCode.E; // Interaction key for the door
    public string ObjectName => "Door";
    void Update()
    {
        if (!doorTransform)
        {
            Debug.LogError("doorTransform is not assigned.");
            return;
        }

        if (isOpen)
        {
            doorTransform.localRotation = Quaternion.Lerp(doorTransform.localRotation, Quaternion.Euler(openRotation), Time.deltaTime * rotationSpeed);
        }
        else
        {
            doorTransform.localRotation = Quaternion.Lerp(doorTransform.localRotation, Quaternion.Euler(closedRotation), Time.deltaTime * rotationSpeed);
        }
        
    }

    private void OpenDoor()
    {
        isOpen = !isOpen; // Toggle the door state
        Debug.Log($"Door state changed: isOpen = {isOpen}");
    }

    public void Interact()
    {
        Debug.Log("Interacting with door");
        OpenDoor();
    }
}