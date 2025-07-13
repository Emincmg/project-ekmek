using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        /// <summary>
        /// Called when a character is made an interaction with the object. Differs every object.
        /// </summary>
        /// <returns></returns>
        void Interact();
        
        /// <summary>
        /// Sets the interaction key.
        /// </summary>
        KeyCode InteractionKey { get; }
        
        /// <summary>
        /// Sets the name of the object.
        /// </summary>
        string ObjectName { get; }
    }
}