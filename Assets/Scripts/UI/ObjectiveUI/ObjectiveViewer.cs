using Interface;
using Manager;
using TMPro;
using UI.Notification;
using UnityEngine;

namespace UI.ObjectiveUI
{
    public class ObjectiveViewer : MonoBehaviour, IEventReceiver
    {
        [SerializeField] TMP_Text m_text;
        private NotificationHandler m_handler;

        private void Awake()
        {
            m_handler = GetComponent<NotificationHandler>();
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            EventManager.OnObjectiveUpdate += EventManager_OnObjectiveUpdate;
        }

        private void EventManager_OnObjectiveUpdate(Levels.Objective objective)
        {
            switch (objective.CompletionState)
            {
                case Levels.Objective.COMPLETION_STATE.START:
                    m_text.text = objective.Description;
                    m_text.color = Color.white;
                    break;
                case Levels.Objective.COMPLETION_STATE.SUCCESS:
                    m_text.color = Color.green;
                    m_handler.PushResultMsg("MISSION SUCCESS", NotificationHandler.PRIORITY.WARNING);
                    break;
                case Levels.Objective.COMPLETION_STATE.FAIL:
                    m_text.color = Color.red;
                    m_handler.PushResultMsg("MISSION FAILED", NotificationHandler.PRIORITY.DANGER);
                    break;
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