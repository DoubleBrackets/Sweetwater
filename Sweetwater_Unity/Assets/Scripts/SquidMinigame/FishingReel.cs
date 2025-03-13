using Interaction;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace SquidMinigame
{
    public class FishingReel : MonoBehaviour
    {
        private enum ReelState
        {
            Idle,
            Casting,
            WaitingForFish,
            Reeling
        }

        [InfoBox("Handles behavior for casting, reeling, and when something is caught")]
        [Header("Dependencies")]

        [SerializeField]
        private SimpleInteractable _interactable;

        [Header("Config")]

        [SerializeField]
        private float _castDuration;

        [SerializeField]
        private float _reelDuration;

        [SerializeField]
        [MinMaxSlider(0f, 20f)]
        private Vector2 _fishPullInterval;

        [Header("Events")]

        public UnityEvent OnItemReeledEvent;

        public UnityEvent OnStartCastingEvent;

        public UnityEvent OnFinishedCastingEvent;

        public UnityEvent OnStartReelingEvent;

        public UnityEvent OnFinishedReelingEvent;

        [Header("Debug")]

        [ShowNonSerializedField]
        private ReelState _reelState = ReelState.Idle;

        [ShowNonSerializedField]
        private float _actionTimer;

        [ShowNonSerializedField]
        private float _fishPullTimer;

        private void Update()
        {
            _actionTimer -= Time.deltaTime;

            switch (_reelState)
            {
                case ReelState.Idle:
                    break;
                case ReelState.Casting:
                    CastingUpdate();
                    break;
                case ReelState.WaitingForFish:
                    break;
                case ReelState.Reeling:
                    ReelingUpdate();
                    break;
            }
        }

        public void DoInteract()
        {
            switch (_reelState)
            {
                case ReelState.Idle:
                    StartCasting();
                    break;
                case ReelState.Casting:
                    break;
                case ReelState.WaitingForFish:
                    StartReeling();
                    break;
                case ReelState.Reeling:
                    break;
            }
        }

        private void StartCasting()
        {
            _reelState = ReelState.Casting;
            _actionTimer = _castDuration;
            _interactable.SetInteractable(false);
            OnStartCastingEvent?.Invoke();
        }

        private void StartReeling()
        {
            _reelState = ReelState.Reeling;
            _actionTimer = _reelDuration;
            _interactable.SetInteractable(false);
            SetRandomFishPullTime();
            OnStartReelingEvent?.Invoke();
        }

        private void CastingUpdate()
        {
            if (_actionTimer <= 0)
            {
                _reelState = ReelState.WaitingForFish;
                OnFinishedCastingEvent?.Invoke();
                _interactable.SetInteractable(true);
                _interactable.SetHint("Reel In");
            }
        }

        private void SetRandomFishPullTime()
        {
            _fishPullTimer = Random.Range(_fishPullInterval.x, _fishPullInterval.y);
        }

        private void ReelingUpdate()
        {
            _fishPullTimer -= Time.deltaTime;
            if (_fishPullTimer <= 0f)
            {
                OnItemReeledEvent?.Invoke();
                SetRandomFishPullTime();
            }

            if (_actionTimer <= 0)
            {
                _reelState = ReelState.Idle;
                OnFinishedReelingEvent?.Invoke();
                _interactable.SetInteractable(true);
                _interactable.SetHint("Cast");
            }
        }
    }
}