using UnityEngine;
using UnityEngine.Serialization;

namespace Interaction
{
    public interface IInteractable
    {
        public InteractionResult Interact(InteractionAttempt attempt);
        
        public CanInteractCheckResult CanInteract(InteractionAttempt attempt);
    }

    public struct CanInteractCheckResult
    {
        public bool CanInteract;
        [FormerlySerializedAs("CursorHInt")]
        [FormerlySerializedAs("InteractionDescription")]
        public string CursorHit;
    }

    public struct InteractionAttempt
    {
        public float InteractionDistance;
    }
    
    public struct InteractionResult
    {
        public bool Success;
    }
}
