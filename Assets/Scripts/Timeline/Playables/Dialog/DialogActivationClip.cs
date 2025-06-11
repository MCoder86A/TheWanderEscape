using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.Playables.Dialog
{
    public class DialogActivationClip : PlayableAsset
    {
        [SerializeField, ResizableTextArea] private string promptText;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<DialogBehaviour> _graph = ScriptPlayable<DialogBehaviour>.Create(graph);
            Debug.Log(nameof(CreatePlayable));
            _graph.GetBehaviour().Prompt = promptText;
            return _graph;
        }
    }
}
