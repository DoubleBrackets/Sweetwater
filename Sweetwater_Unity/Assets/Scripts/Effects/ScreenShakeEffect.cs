using Base;
using Unity.Cinemachine;
using UnityEngine;

namespace Effects
{
    public class ScreenShakeEffect : DescriptionMono
    {
        [Header("Screenshake")]
        [SerializeField]
        private CinemachineImpulseDefinition _screenShakeEffect;

        [SerializeField]
        private Vector3 _shakeVelocity;

        [SerializeField]
        private float _force;
        
        [Header("Playing Behavior")]

        [SerializeField]
        private bool _playOnStart;
        
        [SerializeField]
        private bool _continuous;

        private bool _isPlayingContinuously;

        private bool _isHidden;

        private float _continuousTimer;
        
        private void Start()
        {
            if (_playOnStart)
            {
                Play();
            }
        }

        private void Update()
        {
            if (_isPlayingContinuously)
            {
                _continuousTimer += Time.deltaTime;
                if (_continuousTimer >= _screenShakeEffect.ImpulseDuration)
                {
                    Play();
                    _continuousTimer = 0;
                }
            }
        }

        public void Play()
        {
            if (_isHidden)
            {
                return;
            }
            
            if(_force == 0)
            {
                return;
            }

            _isPlayingContinuously = _continuous;
            _screenShakeEffect.CreateEvent(transform.position, _shakeVelocity * _force);
        }

        public void Stop()
        {
            _isPlayingContinuously = false;
        }

        public void Hide()
        {
            _isHidden = true;
        }

        public void Show()
        {
            _isHidden = false;
        }
    }
}