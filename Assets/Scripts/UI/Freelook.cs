using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utility;

namespace UI
{
    public class Freelook : MonoBehaviour, IDragHandler, IPointerExitHandler
    {
        [SerializeField] private InputActionProperty m_actionLookProperty;
        public static Freelook s_Instance;
        private float
            x = 0,
            y = 0;

        public float X => x;
        public float Y => y;

        private void Awake()
        {
            if (s_Instance == null) s_Instance = this;
            else if (s_Instance != this) Destroy(this);

            RegisterEv();
        }

        private void RegisterEv()
        {
            m_actionLookProperty.action.performed += Look_performed;
        }

        public void OnDrag(PointerEventData eventData)
        {
            x = eventData.delta.x;
            y = eventData.delta.y;
        }

        public void Look_performed(InputAction.CallbackContext p_context)
        {
            Vector2 move = p_context.ReadValue<Vector2>();
            x = move.x;
            y = move.y;
        }

        private void Update()
        {
            if (Touchscreen.current != null && (Touchscreen.current.press.wasReleasedThisFrame || (Touchscreen.current.press.isPressed && !InputEvents.s_Instance.IsDragging))) ResetDelta();
            if (Mouse.current != null && Mouse.current.delta.value != Vector2.zero && !Mouse.current.leftButton.isPressed && Cursor.lockState != CursorLockMode.Locked) ResetDelta();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ResetDelta();
        }

        private void ResetDelta()
        {
            x = y = 0;
        }

        private void UnRegisterEv()
        {
            m_actionLookProperty.action.performed -= Look_performed;
        }

        private void OnDestroy()
        {
            UnRegisterEv();
        }
    }
}