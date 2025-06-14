using UnityEngine;

namespace Interface.Combat
{
    public interface IAttackable
    {
        void Attack(int damage);
        int GetHealth();
        int GetMaxHealth();
        bool IsAlive();
        MonoBehaviour MonoBehaviour { get; }
    }
}