using Game.Managers;
using UnityEngine.Playables;

namespace Timeline.Playables.Dialog
{
    public class DialogBehaviour : PlayableBehaviour
    {
        public string Author;
        public string Prompt;
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            PushDialog(playable);
        }

        private async void PushDialog(Playable playable)
        {
            _ = DialogHandler.Instance.PushPromptText($"{Author}: ", Prompt);
            playable.GetGraph().GetRootPlayable(0).SetSpeed(0);
            await DialogHandler.Instance.WaitForNextAsync("");
            DialogHandler.Instance.ClearDialog();
            playable.GetGraph().GetRootPlayable(0).SetSpeed(1);
        }
    }
}
