using TMPro;
using UnityEngine;

namespace SquidMinigame
{
    public class SquidCargoHold : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _squidCountText;

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
            _currentSquidCount += squidBin.CurrentSquidCount;
            _squidCountText.text = _currentSquidCount.ToString();
            Destroy(squidBin.gameObject);
        }
    }
}