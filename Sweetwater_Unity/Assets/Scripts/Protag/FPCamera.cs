using Base;
using Unity.Cinemachine;
using UnityEngine;

namespace Protag
{
    public class FPCamera : DescriptionMono
    {
        [Header("Dependencies")]

        [SerializeField]
        private CinemachineCamera _cinemachineCamera;

        [SerializeField]
        private CinemachineRecomposer _cinemachineRecomposer;

        [SerializeField]
        private Transform _cameraTransform;

        [Header("Stats")]

        [SerializeField]
        private float _sensitivity;

        [SerializeField]
        private float _moveTilt;

        [SerializeField]
        private float _moveDutch;

        private float _xRotation;
        private float _yRotation;

        private float _adjustTilt;
        private float _adjustDutch;

        public void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            _xRotation -= mouseY * _sensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _yRotation += mouseX * _sensitivity;

            _cameraTransform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);

            // Extra roll and tilt for juice
            if (_cinemachineRecomposer != null)
            {
                JuiceRecompose();
            }
        }

        private void JuiceRecompose()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            float t = 1 - Mathf.Pow(0.01f, Time.deltaTime);
            _adjustTilt = Mathf.Lerp(_adjustTilt, vertical * _moveTilt, t);
            _adjustDutch = Mathf.Lerp(_adjustDutch, -horizontal * _moveDutch, t);

            _cinemachineRecomposer.Tilt = _adjustTilt;
            _cinemachineRecomposer.Dutch = _adjustDutch;
        }
    }
}