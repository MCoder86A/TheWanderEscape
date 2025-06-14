using Interface.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        [SerializeField] private Transform m_healthBarWorldPosition;
        private IAttackable IAttackable;

        private int m_maxHealth;

        private void Awake()
        {
            IAttackable = GetComponentInParent<IAttackable>();
            m_maxHealth = IAttackable.GetMaxHealth();
        }

        private void Update()
        {

            float _currentH = IAttackable.GetHealth() / (float)m_maxHealth;
            if (m_healthBar.fillAmount != _currentH)
            {
                m_healthBar.fillAmount = _currentH;
            }
        }

        private void OnGUI()
        {
            Vector2 _pos = RectTransformUtility.WorldToScreenPoint(Camera.main, m_healthBarWorldPosition.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), _pos, null, out Vector2 _tpos);
            GetComponent<RectTransform>().anchoredPosition = _tpos;
        }
    }
}