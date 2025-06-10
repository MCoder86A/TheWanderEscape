using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controllers
{
    public class ThirdPersonController : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        [SerializeField, AnimatorParam(nameof(m_Animator))] private int m_speedParam;
        [SerializeField, AnimatorParam(nameof(m_Animator))] private int m_jumpParam;

        [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
        public float velocity = 5f;
        [Tooltip("This value is added to the speed value while the character is sprinting.")]
        public float sprintAdittion = 3.5f;
        [Tooltip("The higher the value, the higher the character will jump.")]
        public float jumpForce = 18f;
        [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
        public float jumpTime = 0.85f;
        [Space]
        [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
        public float gravity = 9.8f;

        float jumpElapsedTime = 0;

        // Player states
        bool isJumping = false;
        bool isSprinting = false;
        bool isCrouching = false;

        // Inputs
        float inputHorizontal;
        float inputVertical;
        CharacterController cc;

        InputAction moveAction;
        InputAction sprintAction;
        InputAction jumpAction;

        private void Awake()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            sprintAction = InputSystem.actions.FindAction("Sprint");
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        void Start()
        {
            cc = GetComponent<CharacterController>();
        }


        void Update()
        {
            inputHorizontal = moveAction.ReadValue<Vector2>().x;
            inputVertical = moveAction.ReadValue<Vector2>().y;

            if (cc.isGrounded && m_Animator != null)
            {
                m_Animator.SetFloat(m_speedParam, Mathf.Lerp(m_Animator.GetFloat(m_speedParam), cc.velocity.magnitude, Time.deltaTime * 5));
            }

            if (jumpAction.WasPressedThisFrame())
            {
                isJumping = true;
                m_Animator.SetTrigger(m_jumpParam);
            }

            if (sprintAction.phase == InputActionPhase.Performed)
            {
                isSprinting = true;
            }
            else if(isSprinting) isSprinting = false;

            HeadHittingDetect();

        }


        private void FixedUpdate()
        {
            float velocityAdittion = 0;
            if (isSprinting)
                velocityAdittion = sprintAdittion;
            if (isCrouching)
                velocityAdittion = -(velocity * 0.50f); // -50% velocity

            float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
            float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
            float directionY = 0;

            if (isJumping)
            {
                directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

                jumpElapsedTime += Time.deltaTime;
                if (jumpElapsedTime >= jumpTime)
                {
                    isJumping = false;
                    jumpElapsedTime = 0;
                }
            }

            directionY -= gravity * Time.deltaTime;


            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            forward *= directionZ;
            right *= directionX;

            if (directionX != 0 || directionZ != 0)
            {
                float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
            }

            Vector3 verticalDirection = Vector3.up * directionY;
            Vector3 horizontalDirection = forward + right;

            Vector3 moviment = verticalDirection + horizontalDirection;
            cc.Move(moviment);

        }

        void HeadHittingDetect()
        {
            float headHitDistance = 1.1f;
            Vector3 ccCenter = transform.TransformPoint(cc.center);
            float hitCalc = cc.height / 2f * headHitDistance;

            if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
            {
                jumpElapsedTime = 0;
                isJumping = false;
            }
        }

    }
}