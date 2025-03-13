using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SquidMinigame
{
    public class FishingShiftTimer : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private Slider _shiftTimerSlider;

        [SerializeField]
        private TMP_Text _timeRemainingText;

        [Header("Config")]

        [SerializeField]
        private float _shiftDuration;

        [Header("Events")]

        public UnityEvent OnShiftEnd;

        [ShowNonSerializedField]
        private float _timeLeft;

        private void Start()
        {
            _timeLeft = _shiftDuration;
        }

        private void Update()
        {
            if (_timeLeft <= 0)
            {
                OnShiftEnd?.Invoke();
                return;
            }

            _timeLeft -= Time.deltaTime;
            _shiftTimerSlider.normalizedValue = _timeLeft / _shiftDuration;
            int minutesLeft = Mathf.CeilToInt(_timeLeft / 60);
            _timeRemainingText.text = $"{minutesLeft}";
        }
    }
}