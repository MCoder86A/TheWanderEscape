using Interface;
using Manager;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class ControlCanvas : MonoBehaviour, IEventReceiver
    {
        [SerializeField] private GameObject m_root;

        private void Awake()
        {
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            EventManager.OnTimeLineIntroStarted += EventManager_OnTimeLineIntroStarted;
        }

        private void EventManager_OnTimeLineIntroStarted(bool start)
        {
            m_root.SetActive(!start);
        }

        public void UnRegisterEvents()
        {
            EventManager.OnTimeLineIntroStarted -= EventManager_OnTimeLineIntroStarted;
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }
    }
}