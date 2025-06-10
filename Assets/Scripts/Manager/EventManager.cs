using System;

namespace Manager
{
    public static class EventManager
    {
        public static event Action OnDialogStarted;
        public static void Broadcast_OnDialogStarted()
            => OnDialogStarted?.Invoke();

        public static event Action OnDialogReachedEnd;
        public static void Broadcast_OnDialogReachedEnd()
            => OnDialogReachedEnd?.Invoke();
    }
}