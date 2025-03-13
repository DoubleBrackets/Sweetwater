using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SquidMinigame
{
    public class SquidCargoHold : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private TMP_Text _squidCountText;

        [Header("Events")]

        public UnityEvent<int> OnSquidCountChanged;

        private int _currentSquidCount;

        private void OnTriggerEnter(Collider other)
        {
            var squidBin = other.GetComponentInParent<SquidBin>();
            if (squidBin != null)
            {
                DepositSquidBin(squidBin);
            }
        }

        private void DepositSquidBin(SquidBin squidBin)
        {
            if (squidBin.CurrentSquidCount == 0)
            {
                return;
            }

            _currentSquidCount += squidBin.CurrentSquidCount;
            OnSquidCountChanged?.Invoke(_currentSquidCount);
            _squidCountText.text = _currentSquidCount.ToString();
            Destroy(squidBin.gameObject);
        }
    }
}