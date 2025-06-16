using UnityEngine.InputSystem.Layouts;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace UI.Controls
{
    public class OnScreenSprint : OnScreenControl
    {
        [InputControl(layout = "Button")]
        [SerializeField]
        private string m_ControlPath;

        [SerializeField] private VariableJoystick m_Joystick;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private void OnGUI()
        {
            SendValueToControl(new Vector2(m_Joystick.Horizontal, m_Joystick.Vertical).magnitude > 0.75f ? 1f : 0f);
        }
    }
}
