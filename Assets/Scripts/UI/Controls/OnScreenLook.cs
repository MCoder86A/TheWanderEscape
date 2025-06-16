using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace UI.Controls
{
    public class OnScreenLook : OnScreenControl
    {
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        [SerializeField] private Freelook m_freelook;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private void OnGUI()
        {
            SendValueToControl<Vector2>(new(m_freelook.X, m_freelook.Y));
        }
    }
}