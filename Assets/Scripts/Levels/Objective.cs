using Manager;
using UnityEngine;

namespace Levels
{
    public abstract class Objective : ScriptableObject
    {
        [field: SerializeField] public int Id {  get; private set; }
        [field: SerializeField] public string Description {  get; private set; }

        public COMPLETION_STATE CompletionState { get; protected set; } = COMPLETION_STATE.IDLE;

        public virtual void StartMission()
        {
            CompletionState = COMPLETION_STATE.START;
            EventManager.Broadcast_OnObjectiveUpdate(this);
        }

        public enum COMPLETION_STATE { IDLE, START, FAIL, SUCCESS }
    }
}