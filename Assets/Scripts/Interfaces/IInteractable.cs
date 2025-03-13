using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        void Interact();
        KeyCode InteractionKey { get; }
        string ObjectName { get; }
    }
}