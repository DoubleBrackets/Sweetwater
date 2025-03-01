using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interaction
{
    public class ProtagInteractor : DescriptionMono
    {
        private struct ProbeResult
        {
            public RaycastHit Hit;
            public IInteractable Interactable;
        }
        [SerializeField]
        private Transform _interactCameraTransform;
        
        [SerializeField]
        private LayerMask _interactInterceptLayerMask;

        [Header("Event (In)")]
        [SerializeField]
        private BoolEvent _setInteractionEnabledEvent;
        
        [SerializeField]
        private UnityEvent<CanInteractCheckResult> _canInteractCheckEvent;
        
        private bool _interactEnabled = true;

        private void Awake()
        {
            _setInteractionEnabledEvent.AddListener(HandleInteractionEnabled);
        }
        
        private void OnDestroy()
        {
            _setInteractionEnabledEvent.RemoveListener(HandleInteractionEnabled);
        }
        
        private void HandleInteractionEnabled(bool value)
        {
            _interactEnabled = value;
        }

        private void Update()
        {
            CheckInteractions();
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                AttemptInteract();
            }
        }

        /// <summary>
        ///  Check for interactions to show preview info
        /// </summary>
        private void CheckInteractions()
        {
            var hits = Probe();
            
            if(hits.Count == 0 || !_interactEnabled)
            {
                _canInteractCheckEvent.Invoke(new CanInteractCheckResult()
                {
                    CanInteract = false
                });
                return;
            }
            
            var interactable = hits[0].Interactable;

            var check = interactable.CanInteract(new InteractionAttempt()
            {
                InteractionDistance = hits[0].Hit.distance
            });
            
            _canInteractCheckEvent.Invoke(check);
        }
        
        /// <summary>
        ///  Attempt to interact with the closest interactable
        /// </summary>
        private void AttemptInteract()
        {
            var hits = Probe();
            
            if(hits.Count == 0 || !_interactEnabled)
            {
                return;
            }

            var interactable = hits[0].Interactable;
            var attempt = new InteractionAttempt()
            {
                InteractionDistance = hits[0].Hit.distance
            };
            
            var interactionResult = interactable.Interact(attempt);
        }

        private List<ProbeResult> Probe()
        {
            var ray = new Ray(_interactCameraTransform.position, _interactCameraTransform.forward);
            var hits = Physics.RaycastAll(ray, 1000f, _interactInterceptLayerMask);
            
            var probeResults = new List<ProbeResult>();
            foreach (var hit in hits.OrderBy(hit => hit.distance))
            { 
                var interactable = hit.collider.GetComponent<IInteractable>();
                if(interactable == null)
                {
                    continue;
                }
                
                probeResults.Add(new ProbeResult()
                {
                    Hit = hit,
                    Interactable = interactable
                });
            }
            
            return probeResults;
        }
    }
}
