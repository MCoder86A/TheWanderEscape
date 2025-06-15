using Interface.Combat;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Ely : MonoBehaviour, IAttackable
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private NavMeshAgent m_navAgent;

        [Header("KICK PARAMS")]
        [SerializeField, AnimatorParam(nameof(m_animator))] private int m_kickParam;
        [SerializeField] private float m_kickCooldown = 2;
        [SerializeField] private float m_kickRadius = 1;
        [SerializeField] private Vector3 m_kickOffset;
        [SerializeField] private LayerMask m_kickLayer;
        private float m_kickCooldownTime = 0;

        [Header("COMBAT PARAMS")]
        [SerializeField] private int m_attackRadius = 4;
        [SerializeField] private LayerMask m_attackLayer;
        [SerializeField, AnimatorParam(nameof(m_animator))] private int m_speedParam;

        [Header("MISC")]
        [SerializeField] private int m_maxHealth = 100;
        private int m_health;

        public MonoBehaviour MonoBehaviour => this;

        private void Awake()
        {
            m_health = m_maxHealth;
        }

        public void Attack(int damage)
        {
            m_health = Mathf.Clamp(m_health - damage, 0, m_maxHealth);
        }

        public int GetHealth() => m_health;

        public bool IsAlive() => m_health > 0;

        private void Update()
        {
            if (AttackableUnderAttackRange(out IAttackable attackable) && attackable.IsAlive())
            {
                FollowAttackable(attackable);
                Attack(attackable);
            }

            SyncAnimationParam();

            if (m_kickCooldownTime != 0) m_kickCooldownTime = Mathf.Clamp(m_kickCooldownTime - Time.deltaTime, 0, float.MaxValue);
        }

        private void SyncAnimationParam()
        {
            m_animator.SetFloat(m_speedParam,
                Mathf.Lerp(m_animator.GetFloat(m_speedParam), m_navAgent.velocity.magnitude, Time.deltaTime * 5));
        }

        private bool AttackableUnderAttackRange(out IAttackable p_attackable)
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, m_attackRadius, m_attackLayer);
            for(int i = 0; i< collider.Length; i++)
            {
                if (collider[i].TryGetComponent(out IAttackable attackable))
                {
                    p_attackable = attackable;
                    return true;
                }
            }
            p_attackable = null;
            return false;
        }

        private void FollowAttackable(IAttackable attackable)
        {
            m_navAgent.SetDestination(attackable.MonoBehaviour.transform.position);
            if(m_navAgent.velocity.magnitude <= 0.5f)
            {
                if(m_navAgent.updateRotation) m_navAgent.updateRotation = false;
                transform.forward = Vector3.Slerp(transform.forward, (attackable.MonoBehaviour.transform.position - transform.position).normalized, Time.deltaTime * 5);
            }
            else 
                if(!m_navAgent.updateRotation) m_navAgent.updateRotation = true;
        }

        private void Attack(IAttackable attackable)
        {
            if(m_kickCooldownTime == 0 &&
                Physics.CheckSphere(transform.position + transform.rotation * m_kickOffset, m_kickRadius, m_kickLayer))
            {
                Kick(attackable);
                m_kickCooldownTime = m_kickCooldown;
            }
        }

        private void Kick(IAttackable attackable)
        {
            m_animator.SetTrigger(m_kickParam);
            attackable.Attack(5);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_attackRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.rotation * m_kickOffset, m_kickRadius);
        }

        public int GetMaxHealth() => m_maxHealth;
    }
}