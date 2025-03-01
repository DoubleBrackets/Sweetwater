using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.EventHook
{
    public class EventHookPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [NoFoldOut]
        public EventHookBehavior template = new EventHookBehavior();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<EventHookBehavior>.Create(graph, template);
        }
    }
}