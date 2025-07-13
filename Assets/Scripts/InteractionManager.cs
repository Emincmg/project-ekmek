using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using UnityEngine.Serialization;

public class InteractionManager : MonoBehaviour
{
    [FormerlySerializedAs("InteractorSource")] public Transform interactorSource;
    [FormerlySerializedAs("InteractRange")] public float interactRange = 2f;
    public CanvasGroup interactorCanvasGroup;
    void Update()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactableObj))
            {
                if (Input.GetKeyDown(interactableObj.InteractionKey))
                {
                    interactableObj.Interact();
                }
            }
        }
    }
}