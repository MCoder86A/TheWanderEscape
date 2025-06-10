using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Dialog
{
    public class Option : MonoBehaviour
    {
        private Button m_button;
        public Button Button
        {
            get
            {
                if(m_button == null) return m_button = GetComponent<Button>();
                return m_button;
            }
        }
        [field: SerializeField] public TMP_Text Text {  get; private set; }

        private void Awake()
        {
            Text = GetComponentInChildren<TMP_Text>(true);
        }
    }
}