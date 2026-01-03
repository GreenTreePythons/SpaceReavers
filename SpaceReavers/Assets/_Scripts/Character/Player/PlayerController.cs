using UnityEngine;

namespace _Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float m_MoveSpeed = 2.0f;
        
        private PlayerInputController m_PlayerInputController;
        private PlayerStateController m_PlayerStateController;
        private CharacterController m_CharacterController;
        
        private Vector3 m_MoveDirection = Vector3.zero;
        private Transform m_CameraTransform;
        
        private void Awake()
        {
            m_PlayerInputController = GetComponent<PlayerInputController>();
            m_PlayerStateController = GetComponent<PlayerStateController>();
            m_CharacterController = GetComponent<CharacterController>();
            m_CameraTransform = Camera.main.transform;
        }
        
        private void Update()
        {
            var inputDirection = m_PlayerInputController.InputDirection;
            Vector3 forward = m_CameraTransform.forward;
            forward.y = 0f;

            Vector3 right = m_CameraTransform.right;
            right.y = 0f;
            
            m_MoveDirection = Vector3.ClampMagnitude(right * inputDirection.x + forward * inputDirection.y, 1f);
            if (m_MoveDirection.magnitude > 0f)
            {
                m_PlayerStateController.ChangeState(PlayerStateType.Move);
                Quaternion targetRotation = Quaternion.LookRotation(m_MoveDirection);
                Quaternion nextQuaternion = Quaternion.Slerp(transform.rotation, targetRotation, 0.3f);
                transform.rotation = nextQuaternion;
            }
            else
            {
                m_PlayerStateController.ChangeState(PlayerStateType.Idle);
            }

            m_PlayerStateController.UpdateState();
        }

        private void FixedUpdate()
        {
            m_CharacterController.Move(m_MoveDirection * m_MoveSpeed * Time.fixedDeltaTime);
        }
    }
}