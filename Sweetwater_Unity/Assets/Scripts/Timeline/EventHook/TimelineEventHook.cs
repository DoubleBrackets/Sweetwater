using Base;
using UnityEngine.Events;

namespace Timeline.EventHook
{
    /// <summary>
    ///  Generic monobehavior that hooks into timeline clips and emits unity events
    /// </summary>
    public class TimelineEventHook : DescriptionMono
    {
        public UnityEvent OnClipStart;
        public UnityEvent OnClipEnd;
        public UnityEvent<float> OnClipEvaluateNormalized;

        public void InvokeOnClipStart()
        {
            OnClipStart?.Invoke();
        }

        public void InvokeOnClipEnd()
        {
            OnClipEnd?.Invoke();
        }

        public void InvokeOnClipEvaluateNormalized(float normalizedTime)
        {
            OnClipEvaluateNormalized?.Invoke(normalizedTime);
        }
    }
}