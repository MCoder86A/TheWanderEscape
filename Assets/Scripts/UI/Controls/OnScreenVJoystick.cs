using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace UI.Controls
{
    public class OnScreenVJoystick : OnScreenControl
    {
        [InputControl(layout = "Vector2")]
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
            SendValueToControl<Vector2>(new (m_Joystick.Horizontal, m_Joystick.Vertical));
        }
    }
}