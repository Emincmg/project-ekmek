using UnityEngine;
using Interfaces;

public class DoorScript : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Determines if the door is open or not. You can set initial state.
    /// </summary>
    public bool isOpen = false;

    /// <summary>
    /// Transform object of the door.
    /// </summary>
    public Transform doorTransform;
    
    /// <summary>
    /// Speed of the rotation of the door object.
    /// </summary>
    public float rotationSpeed = 20f; 
    
    /// <summary>
    /// Rotation of the door when its open. You must set this to the correct one otherwise it will be goofy.
    /// </summary>
    public Vector3 openRotation = new Vector3(0, 0, 0); 
    
    /// <summary>
    /// Rotation of the door when its closed. You must set this to the correct one otherwise it will be goofy.
    /// </summary>
    public Vector3 closedRotation = new Vector3(0, 0, 0);
    
    /// <summary>
    /// Interaction key for the door to be interactable.
    /// </summary>
    public KeyCode InteractionKey => KeyCode.E; 
    
    /// <summary>
    /// Name of the object. Comes from IInteractable interface.
    /// </summary>
    public string ObjectName => "Door";
    
    /// <summary>
    /// Very costly. Thread lightly.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Transforms the door when its called.
    /// </summary>
    /// <returns></returns>
    private void OpenDoor()
    {
        isOpen = !isOpen; 
    }

    /// <summary>
    /// Interaction function. Comes from IInteractable interface.
    /// </summary>
    /// <returns></returns>
    public void Interact()
    {
        OpenDoor();
    }
}