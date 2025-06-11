using Manager;
using UnityEngine;
using UnityEngine.Playables;

namespace Timelines
{
    public class TimeLine : MonoBehaviour
    {
        protected PlayableDirector m_Director;
        virtual protected void Awake()
        {
            m_Director = GetComponent<PlayableDirector>();
        }

        public void OnStart()
        {
            EventManager.Broadcast_OnTimeLineIntroStarted(true);
            OnTimelineStart();
        }

        public void OnEnd()
        {
            EventManager.Broadcast_OnTimeLineIntroStarted(false);
            OnTimelineEnd();
        }

        public void OnPause()
        {
            EventManager.Broadcast_OnTimeLineIntroStarted(false);
            OnTimelinePause();
        }

        public void OnResume()
        {
            EventManager.Broadcast_OnTimeLineIntroStarted(true);
            OnTimelineResume();
        }

        virtual protected void OnTimelineStart() { }

        virtual protected void OnTimelineEnd() { }

        virtual protected void OnTimelinePause() { }

        virtual protected void OnTimelineResume() { }
    }
}