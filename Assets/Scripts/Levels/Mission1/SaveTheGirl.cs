using Manager;
using NPC;
using Players;
using UnityEngine;

namespace Levels.Mission1
{
    [CreateAssetMenu(fileName = nameof(SaveTheGirl), menuName = "ScriptableObjects/" + nameof(Objective) +"/"+ nameof(SaveTheGirl))]
    public class SaveTheGirl : Objective
    {
        public override void StartMission()
        {
            base.StartMission();
            EventManager.OnSomeoneDie += EventManager_OnSomeoneDie;
            CompletionState = COMPLETION_STATE.START;
            EventManager.Broadcast_OnObjectiveUpdate(this);
        }

        private void EventManager_OnSomeoneDie(Interface.Combat.IAttackable attackable)
        {
            EventManager.OnSomeoneDie -= EventManager_OnSomeoneDie;
            if (attackable is Player) CompletionState = COMPLETION_STATE.FAIL;
            else if (attackable is Ely) CompletionState = COMPLETION_STATE.SUCCESS;
            EventManager.Broadcast_OnObjectiveUpdate(this);
        }

        private void OnDestroy()
        {
            EventManager.OnSomeoneDie -= EventManager_OnSomeoneDie;
        }
    }
}