using UnityEngine;

namespace _Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float m_MoveSpeed = 2.0f;
        
        private PlayerInputReader m_PlayerInputReader;
        private CharacterController m_CharacterController;
        
        private Vector3 m_MoveDirection = Vector3.zero;
        private Transform m_CameraTransform;
        
        private void Awake()
        {
            m_PlayerInputReader = GetComponent<PlayerInputReader>();
            m_CharacterController = GetComponent<CharacterController>();
            m_CameraTransform = Camera.main.transform;
        }
        
        void Update()
        {
            var inputDirection = m_PlayerInputReader.InputDirection;
            Vector3 forward = m_CameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = m_CameraTransform.right;
            right.y = 0f;
            right.Normalize();
            
            m_MoveDirection = right * inputDirection.x + forward * inputDirection.y;
            if (m_MoveDirection.magnitude <= 0) return;
            
            Quaternion targetRotation = Quaternion.LookRotation(m_MoveDirection);
            Quaternion nextQuaternion = Quaternion.Slerp(transform.rotation, targetRotation, 0.3f);
            transform.rotation = nextQuaternion;
        }

        private void FixedUpdate()
        {
            if (m_MoveDirection.magnitude <= 0) return;
            m_CharacterController.Move(m_MoveDirection * m_MoveSpeed * Time.fixedDeltaTime);
        }
    }
}