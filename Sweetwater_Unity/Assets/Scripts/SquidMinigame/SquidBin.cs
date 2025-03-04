using Base;
using Interaction;
using UnityEngine;

namespace SquidMinigame
{
    public class SquidBin : DescriptionMono
    {
        [SerializeField]
        private int _maxSquidCount;

        [SerializeField]
        private SimpleInteractable _interactable;

        public int CurrentSquidCount => _currentSquidCount;

        private int _currentSquidCount;

        private void Awake()
        {
            _interactable.SetHint($"{_currentSquidCount}/{_maxSquidCount}");
        }

        private void OnCollisionEnter(Collision other)
        {
            var squid = other.collider.GetComponentInParent<Squid>();
            if (squid != null)
            {
                Destroy(squid.gameObject);
                AddSquid();
            }
        }

        private void AddSquid()
        {
            if (_currentSquidCount >= _maxSquidCount)
            {
                Debug.Log("Squid bin full");
            }

            _currentSquidCount++;

            _interactable.SetHint($"{_currentSquidCount}/{_maxSquidCount}");
        }
    }
}