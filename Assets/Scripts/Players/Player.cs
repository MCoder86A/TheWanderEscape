using Interface.Combat;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Players
{
    public class Player : MonoBehaviour, IAttackable
    {
        [SerializeField] private Animator m_animator;

        [Header("KICK PARAMS")]
        [SerializeField, AnimatorParam(nameof(m_animator))] private int m_kickParam;
        [SerializeField] private float m_kickCooldown = 2;
        private float m_kickCooldownTime = 0;

        [SerializeField] private float m_kickRadius = 1;
        [SerializeField] private Vector3 m_kickOffset;
        [SerializeField] private LayerMask m_kickLayer;

        [Header("INPUT ACTIONS")]
        [SerializeField] private InputActionProperty m_kickAction;

        [Header("MISC")]
        [SerializeField] private int m_maxHealth = 100;
        private int m_health;

        public MonoBehaviour MonoBehaviour => this;

        private void Awake()
        {
            m_health = m_maxHealth;
            m_kickAction.action.performed += Action_kickPerformed;
        }

        private void Update()
        {
            if(m_kickCooldownTime != 0) m_kickCooldownTime = Mathf.Clamp(m_kickCooldownTime-Time.deltaTime, 0, float.MaxValue);
        }

        private void Action_kickPerformed(InputAction.CallbackContext _)
        {
            if(m_kickCooldownTime == 0)
            {
                m_kickCooldownTime = m_kickCooldown;
                m_animator.SetTrigger(m_kickParam);

                Collider[] collider = Physics.OverlapSphere(transform.position + transform.rotation * m_kickOffset, m_kickRadius, m_kickLayer);
                if (collider != null)
                {
                    for(int i=0; i<collider.Length; i++)
                    {
                        if (collider[i].TryGetComponent(out IAttackable attackable))
                        {
                            attackable.Attack(5);
                        }
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.rotation * m_kickOffset, m_kickRadius);
        }

        public void Attack(int damage)
        {
            m_health = Mathf.Clamp(m_health - damage, 0, m_maxHealth);
        }

        public int GetHealth() => m_health;

        public bool IsAlive() => m_health > 0;

        public int GetMaxHealth() => m_maxHealth;
    }
}