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

        [FormerlySerializedAs("CursorHit")]
        [FormerlySerializedAs("CursorHInt")]
        [FormerlySerializedAs("InteractionDescription")]
        public string CursorHint;
    }

    public struct InteractionAttempt
    {
        public float InteractionDistance;
    }

    public struct InteractionResult
    {
        public bool Success;

        /// <summary>
        ///     The transform of the object that was picked up, if any
        /// </summary>
        public Transform PickupTransform;

        /// <summary>
        ///     The pose of how the object will be held
        /// </summary>
        public Pose PickupPose;
    }
}