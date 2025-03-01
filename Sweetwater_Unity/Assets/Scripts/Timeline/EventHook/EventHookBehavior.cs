using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.EventHook
{
    [Serializable]
    public class EventHookBehavior : PlayableBehaviour
    {
        private TimelineEventHook eventHookHandler;

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            if (eventHookHandler != null)
            {
                eventHookHandler.InvokeOnClipEnd();
            }
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            eventHookHandler = info.output.GetUserData() as TimelineEventHook;

            if (eventHookHandler == null)
            {
                 Debug.LogWarning("Missing Timeline Event Hook binding");
                 return;
            }
            
            eventHookHandler.InvokeOnClipStart();
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            var t = playable.GetTime() / playable.GetDuration();
            eventHookHandler.InvokeOnClipEvaluateNormalized((float)t);
        }

       
    }
}