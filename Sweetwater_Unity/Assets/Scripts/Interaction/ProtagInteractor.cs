using System.Collections.Generic;
using System.Linq;
using Base;
using Events;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class ProtagInteractor : DescriptionMono
    {
        private struct ProbeResult
        {
            public IInteractable Interactable;
            public RaycastHit Hit;
        }

        [Header("Interaction Config")]

        [SerializeField]
        private Transform _interactCameraTransform;

        [SerializeField]
        private LayerMask _interactInterceptLayerMask;

        [Header("Depends")]

        [SerializeField]
        private ProtagPickup _protagPickup;

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

        private void Update()
        {
            CheckInteractions();
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                AttemptInteract();
            }
        }

        private void OnDestroy()
        {
            _setInteractionEnabledEvent.RemoveListener(HandleInteractionEnabled);
        }

        private void HandleInteractionEnabled(bool value)
        {
            _interactEnabled = value;
        }

        /// <summary>
        ///     Check for interactions to show preview info
        /// </summary>
        private void CheckInteractions()
        {
            if (!_interactEnabled)
            {
                _canInteractCheckEvent.Invoke(new CanInteractCheckResult
                {
                    CanInteract = false
                });
                return;
            }

            if (_protagPickup.IsHoldingObject)
            {
                _canInteractCheckEvent.Invoke(new CanInteractCheckResult
                {
                    CanInteract = true,
                    CursorHint = "Drop"
                });
                return;
            }

            List<ProbeResult> hits = Probe();

            if (hits.Count == 0)
            {
                _canInteractCheckEvent.Invoke(new CanInteractCheckResult
                {
                    CanInteract = false
                });
                return;
            }

            IInteractable interactable = hits[0].Interactable;

            CanInteractCheckResult check = interactable.CanInteract(new InteractionAttempt
            {
                InteractionDistance = hits[0].Hit.distance
            });

            _canInteractCheckEvent.Invoke(check);
        }

        /// <summary>
        ///     Attempt to interact with the closest interactable
        /// </summary>
        private void AttemptInteract()
        {
            if (!_interactEnabled)
            {
                return;
            }

            if (_protagPickup.IsHoldingObject)
            {
                _protagPickup.DropObject();
                return;
            }

            List<ProbeResult> hits = Probe();

            if (hits.Count == 0)
            {
                return;
            }

            IInteractable interactable = hits[0].Interactable;
            var attempt = new InteractionAttempt
            {
                InteractionDistance = hits[0].Hit.distance
            };

            InteractionResult interactionResult = interactable.Interact(attempt);

            if (interactionResult.Success)
            {
                if (interactionResult.PickupTransform != null)
                {
                    _protagPickup.PickupObject(interactionResult.PickupTransform, interactionResult.PickupPose);
                }
            }
        }

        private List<ProbeResult> Probe()
        {
            var ray = new Ray(_interactCameraTransform.position, _interactCameraTransform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, _interactInterceptLayerMask);

            var probeResults = new List<ProbeResult>();
            foreach (RaycastHit hit in hits.OrderBy(hit => hit.distance))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable == null)
                {
                    continue;
                }

                probeResults.Add(new ProbeResult
                {
                    Hit = hit,
                    Interactable = interactable
                });
            }

            return probeResults;
        }
    }
}