using Interface;
using Manager;
using Timelines;

namespace Levels.Mission1
{
    public class ObjectivePass : TimeLine, IEventReceiver
    {
        protected override void Awake()
        {
            base.Awake();
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            EventManager.OnObjectiveUpdate += EventManager_OnObjectiveUpdate;
        }

        private void EventManager_OnObjectiveUpdate(Objective objective)
        {
            if(objective is SaveTheGirl && objective.CompletionState == Objective.COMPLETION_STATE.SUCCESS)
            {
                m_Director.Play();
            }
        }

        public void UnRegisterEvents()
        {
            EventManager.OnObjectiveUpdate -= EventManager_OnObjectiveUpdate;
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }

    }
}