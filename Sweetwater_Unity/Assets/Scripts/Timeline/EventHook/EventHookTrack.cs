using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.EventHook
{
    [TrackColor(0f, 0.5f, 1f)]
    [TrackClipType(typeof(EventHookPlayableAsset))]
    [TrackBindingType(typeof(TimelineEventHook))]
    public class EventHookTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<EventHookMixerBehavior>.Create(graph, inputCount);
        }
        
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
           
            base.GatherProperties(director, driver);
        }
    }
    
    public class EventHookMixerBehavior : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}
