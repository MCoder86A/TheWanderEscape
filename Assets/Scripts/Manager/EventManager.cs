using Interface.Combat;
using Levels;
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

        public static event Action<bool> OnTimeLineIntroStarted;
        public static void Broadcast_OnTimeLineIntroStarted(bool p_enable)
            => OnTimeLineIntroStarted?.Invoke(p_enable);

        public static event Action<IAttackable> OnSomeoneDie;
        public static void Broadcast_OnSomeoneDie(IAttackable p_attackable)
            => OnSomeoneDie?.Invoke(p_attackable);

        public static event Action<Objective> OnObjectiveUpdate;
        public static void Broadcast_OnObjectiveUpdate(Objective p_objective)
            => OnObjectiveUpdate?.Invoke(p_objective);
    }
}