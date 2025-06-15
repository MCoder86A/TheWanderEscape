using Manager;
using Timelines;
using UI.Notification;

namespace Levels.Mission1
{
    public class ObjectiveIntro : TimeLine
    {
        protected override void OnTimelineEnd()
        {
            base.OnTimelineEnd();
            ObjectiveHandler.Instance.NextObjective();
            NotificationHandler.Instance.PushResultMsg("MISSION STARTED", NotificationHandler.PRIORITY.WARNING);
        }
    }
}