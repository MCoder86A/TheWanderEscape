using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField, AnimatorParam(nameof(m_Animator))] private int m_speedParam;
    [SerializeField, AnimatorParam(nameof(m_Animator))] private int m_sideSpeedParam;

    [SerializeField] private float speedFactor = 10;
    [SerializeField] private float sideSpeedFactor = 10;

    public CharacterController controller;

    public float speed = 5;
    public float gravity = -9.18f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    InputAction moveAction;
    InputAction jumpAction;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame && isGrounded)
        {
            speed = 10;
        }
        else
        {
            speed = 5;
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float x = moveValue.x;
        float z = moveValue.y;

        if (moveValue != Vector2.zero) { transform.forward = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * Vector3.forward; }
        Vector3 move = transform.right * x + transform.forward * z;
        //if (moveValue != Vector2.zero) { transform.forward = move.normalized; }
        print(move);
        //controller.Move(move * speed * Time.deltaTime);
        m_Animator.SetFloat(m_speedParam, move.magnitude * speed);
        m_Animator.SetFloat(m_sideSpeedParam, (transform.right * x).magnitude);

        if (jumpAction.phase == InputActionPhase.Performed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}