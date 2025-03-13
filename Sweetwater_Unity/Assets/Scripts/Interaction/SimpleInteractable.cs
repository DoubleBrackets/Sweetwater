using Base;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class SimpleInteractable : DescriptionMono, IInteractable
    {
        [Header("Interaction Config")]

        [SerializeField]
        private float _interactionDistance;

        [SerializeField]
        private string _cursorHint;

        [SerializeField]
        private bool _oneTimeUse;

        [Header("Pickup")]

        [SerializeField]
        private bool _pickupable;

        [SerializeField]
        private Transform _pickupTransform;

        [SerializeField]
        private Pose _pickupPose;

        public UnityEvent<InteractionAttempt> OnInteractEvent;

        [Header("Debug")]

        [ShowNonSerializedField]
        private bool _used;

        [ShowNonSerializedField]
        private bool _interactable = true;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _interactionDistance);

            if (_pickupable && _pickupTransform != null)
            {
                Gizmos.color = Color.yellow;
                Vector3 pos = _pickupTransform.rotation * Quaternion.Inverse(_pickupPose.rotation) *
                              _pickupPose.position;
                Vector3 position = _pickupTransform.position;
                Vector3 relativeHoldPos = position - pos;
                Gizmos.DrawWireSphere(relativeHoldPos, 0.1f);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(relativeHoldPos, relativeHoldPos + _pickupPose.rotation * Vector3.forward);
                Gizmos.color = Color.green;

                Gizmos.DrawLine(relativeHoldPos, relativeHoldPos + _pickupPose.rotation * Vector3.down);
            }
        }

        public InteractionResult Interact(InteractionAttempt attempt)
        {
            if (_oneTimeUse && _used)
            {
                return new InteractionResult
                {
                    Success = false
                };
            }

            if (attempt.InteractionDistance > _interactionDistance)
            {
                return new InteractionResult
                {
                    Success = false
                };
            }

            OnInteractEvent.Invoke(attempt);

            _used = true;

            var result = new InteractionResult
            {
                Success = true
            };

            if (_pickupable)
            {
                if (_pickupTransform == null)
                {
                    Debug.LogWarning("No pickup transform set", gameObject);
                }
                else
                {
                    result.PickupTransform = _pickupTransform;
                    result.PickupPose = _pickupPose;
                }
            }

            return result;
        }

        public CanInteractCheckResult CanInteract(InteractionAttempt attempt)
        {
            if (!_interactable)
            {
                return new CanInteractCheckResult
                {
                    CanInteract = false
                };
            }

            if (_oneTimeUse && _used)
            {
                return new CanInteractCheckResult
                {
                    CanInteract = false
                };
            }

            if (attempt.InteractionDistance > _interactionDistance)
            {
                return new CanInteractCheckResult
                {
                    CanInteract = false
                };
            }

            return new CanInteractCheckResult
            {
                CanInteract = true,
                CursorHint = _cursorHint
            };
        }

        public void SetHint(string hint)
        {
            _cursorHint = hint;
        }

        public void ResetOneTimeUse()
        {
            _used = false;
        }

        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;
        }
    }
}