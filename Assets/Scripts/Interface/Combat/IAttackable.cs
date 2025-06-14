using UnityEngine;

namespace Interface.Combat
{
    public interface IAttackable
    {
        void Attack(int damage);
        int GetHealth();
        bool IsAlive();
        MonoBehaviour MonoBehaviour { get; }
    }
}