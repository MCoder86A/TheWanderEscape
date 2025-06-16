using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utility
{
    public class InputEvents : MonoBehaviour
    {
        [Space(5), Header(nameof(InputEvents))]
        [Range(5, 100), SerializeField] private float m_dragThreshold = 50;
        [SerializeField] private bool m_isDraged;
        [SerializeField] private bool m_isDragging;
        [SerializeField] private bool m_isLongPress;
        [SerializeField] private float m_longPressDuration;

        public static InputEvents s_Instance;

        public event Action<Vector2> OnLongPress;
        public event Action OnLeftMouseButtonDown;
        public event Action OnLeftMouseButtonUp;
        public event Action OnLeftMouseButtonClick;
        public bool IsDraged => m_isDraged;
        public bool IsDragging => m_isDragging;
        public bool IsLongPress => m_isLongPress;

        private Vector2 m_start_pos;
        private float m_longPressTimer = 0;
        private Vector2 m_last_pos;

        private void Awake()
        {
            s_Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (Pointer.current.press.wasPressedThisFrame)
            {
                m_start_pos = Pointer.current.position.value;
                m_longPressTimer = 0;
                OnLeftMouseButtonDown?.Invoke();
            }
            if (Pointer.current.press.isPressed)
            {
                DetectDrag();
                DetectLongPress();
            }
            if (Pointer.current.press.wasReleasedThisFrame)
            {
                if (!m_isDraged && !m_isLongPress) { OnLeftMouseButtonClick?.Invoke(); }
                m_isDraged = false;
                m_isLongPress = false;
                OnLeftMouseButtonUp?.Invoke();
            }

            DetectDragging();
        }

        private void DetectDragging()
        {
            if (Touchscreen.current != null)
            {
                bool a_dragging = false;
                for (int i = 0; i < Touchscreen.current.touches.Count; i++)
                {
                    a_dragging = a_dragging
                        || Touchscreen.current.touches[i].delta.value != Vector2.zero;
                }
                m_isDragging = a_dragging;
            }
            else
            {
                if (Pointer.current.delta.value != Vector2.zero) m_isDragging = true;
                else m_isDragging = false;
                m_last_pos = Pointer.current.position.value;
            }
        }

        private void DetectDrag()
        {
            if (m_isDraged == true) return;
            if (Touchscreen.current != null)
            {
                bool a_dragging = false;
                for (int i = 0; i < Touchscreen.current.touches.Count; i++)
                {
                    a_dragging = a_dragging
                        || Vector2.Distance(Touchscreen.current.touches[i].position.value, Touchscreen.current.touches[i].startPosition.value) > m_dragThreshold;
                }
                m_isDraged = a_dragging;
            }
            else
            {
                if (Vector2.Distance(m_start_pos, Pointer.current.position.value) > m_dragThreshold) m_isDraged = true;
            }
        }
        private void DetectLongPress()
        {
            if (m_longPressTimer > m_longPressDuration && !IsDraged && !m_isLongPress)
            {
                m_isLongPress = true;
                OnLongPress?.Invoke(m_start_pos);
            }
            m_longPressTimer += Time.deltaTime;
        }
    }
}