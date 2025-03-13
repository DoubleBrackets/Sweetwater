using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SquidMinigame
{
    public class SquidingProgressBar : MonoBehaviour
    {
        [Header("Events (In)")]

        [SerializeField]
        private IntEvent onSquidCountChanged;

        [Header("Depends")]

        [SerializeField]
        private Slider progressBar;

        [SerializeField]
        private TMP_Text squidCountText;

        [Header("Config")]

        [SerializeField]
        private int desiredSquidQuota;

        [Header("Event")]

        public UnityEvent<int> OnSquidCountChanged;

        private int _currentSquidCount;

        private void Start()
        {
            onSquidCountChanged.AddListener(UpdateProgressBar);
        }

        private void OnDestroy()
        {
            onSquidCountChanged.RemoveListener(UpdateProgressBar);
        }

        public void UpdateProgressBar(int newSquidCount)
        {
            if (newSquidCount == _currentSquidCount)
            {
                return;
            }

            progressBar.normalizedValue = (float)newSquidCount / desiredSquidQuota;
            squidCountText.text = $"{newSquidCount}/{desiredSquidQuota}";
            OnSquidCountChanged?.Invoke(newSquidCount);
        }
    }
}