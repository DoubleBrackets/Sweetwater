using Interaction;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace SquidMinigame
{
    public class SquidBin : MonoBehaviour
    {
        [InfoBox("Bin that collects Squid interactable objects. Drop into SquidCargo")]
        [Header("Depends")]

        [SerializeField]
        private SimpleInteractable _interactable;

        [Header("Config")]

        [SerializeField]
        private int _maxSquidCount;

        [Header("Events")]

        public UnityEvent<int> OnSquidCountChanged;

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

            OnSquidCountChanged?.Invoke(_currentSquidCount);

            _interactable.SetHint($"{_currentSquidCount}/{_maxSquidCount}");
        }
    }
}