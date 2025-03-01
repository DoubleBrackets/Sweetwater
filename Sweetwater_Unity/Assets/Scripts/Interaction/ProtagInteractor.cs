using System;
using System.Collections.Generic;
using System.Linq;
using Base;
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
        
        [SerializeField]
        private bool _debug;
        
        [SerializeField]
        private UnityEvent<CanInteractCheckResult> _canInteractCheckEvent;
        
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
            
            if(hits.Count == 0)
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
            
            if(hits.Count == 0)
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
