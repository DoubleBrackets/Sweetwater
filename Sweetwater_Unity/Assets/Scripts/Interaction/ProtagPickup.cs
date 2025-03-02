using Base;
using UnityEngine;

namespace Interaction
{
    public class ProtagPickup : DescriptionMono
    {
        [Header("Config")]

        [SerializeField]
        private Transform _holdPoint;

        [SerializeField]
        private FPController _fpController;

        [Header("Debug")]

        [SerializeField]
        private Transform _heldObject;

        public bool IsHoldingObject => _heldObject != null;

        private RigidbodyConstraints _heldObjectConstraints;

        private void Awake()
        {
            _heldObject = null;
        }

        private void OnDrawGizmos()
        {
            if (_holdPoint == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_holdPoint.position, 0.1f);
        }

        public void PickupObject(Transform objectToPickup, Pose holdPose)
        {
            if (_heldObject != null)
            {
                Debug.LogWarning("Already holding an object");
                return;
            }

            _heldObject = objectToPickup;
            _heldObject.SetParent(_holdPoint);
            _heldObject.localPosition = holdPose.position;
            _heldObject.localRotation = holdPose.rotation;

            var objectRb = _heldObject.GetComponentInChildren<Rigidbody>();
            Collider[] colliders = _heldObject.GetComponentsInChildren<Collider>();

            foreach (Collider coll in colliders)
            {
                coll.enabled = false;
            }

            if (objectRb != null)
            {
                _heldObjectConstraints = objectRb.constraints;
                objectRb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        public void DropObject()
        {
            if (_heldObject == null)
            {
                Debug.LogWarning("No object to drop");
                return;
            }

            var objectRb = _heldObject.GetComponentInChildren<Rigidbody>();

            Collider[] colliders = _heldObject.GetComponentsInChildren<Collider>();

            foreach (Collider coll in colliders)
            {
                coll.enabled = true;
            }

            if (objectRb != null)
            {
                objectRb.constraints = _heldObjectConstraints;
                objectRb.linearVelocity = _fpController.Velocity;
            }

            _heldObject.SetParent(null);
            _heldObject = null;
        }
    }
}