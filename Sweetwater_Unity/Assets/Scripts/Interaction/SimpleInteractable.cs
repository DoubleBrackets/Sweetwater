using System;
using Base;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class SimpleInteractable : DescriptionMono, IInteractable
    {
        [SerializeField]
        private float _interactionDistance;

        [SerializeField]
        private string _cursorHint;
        
        [SerializeField]
        private bool _oneTimeUse;
        
        public UnityEvent<InteractionAttempt> OnInteractEvent;
        
        private bool _used;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _interactionDistance);
        }
        
        public void ResetOneTimeUse()
        {
            _used = false;
        }

        public InteractionResult Interact(InteractionAttempt attempt)
        {
            if (_oneTimeUse && _used)
            {
                return new InteractionResult()
                {
                    Success = false
                };
            }
            
            if (attempt.InteractionDistance > _interactionDistance)
            {
                return new InteractionResult()
                {
                    Success = false
                };
            }
            
            OnInteractEvent.Invoke(attempt);
            
            _used = true;
            
            return new InteractionResult()
            {
                Success = true
            };
        }

        public CanInteractCheckResult CanInteract(InteractionAttempt attempt)
        {
            if (_oneTimeUse && _used)
            {
                return new CanInteractCheckResult()
                {
                    CanInteract = false,
                };
            }
            
            if (attempt.InteractionDistance > _interactionDistance)
            {
                return new CanInteractCheckResult()
                {
                    CanInteract = false,
                };
            }

            return new CanInteractCheckResult()
            {
                CanInteract = true,
                CursorHit = _cursorHint
            };
        }
    }
}
