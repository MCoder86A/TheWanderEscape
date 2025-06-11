using Game.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.Playables.Dialog
{
    public class DialogBehaviour : PlayableBehaviour
    {
        public string Prompt;
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            Debug.Log($"{nameof(OnBehaviourPlay)} : {Prompt}");
            PushDialog(playable);
        }

        private async void PushDialog(Playable playable)
        {
            _ = DialogHandler.Instance.PushPromptText("Mira: ", Prompt);
            playable.GetGraph().GetRootPlayable(0).SetSpeed(0);
            await DialogHandler.Instance.WaitForNextAsync("");
            DialogHandler.Instance.ClearDialog();
            playable.GetGraph().GetRootPlayable(0).SetSpeed(1);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            Debug.Log(nameof(OnBehaviourPause));
        }
    }
}
