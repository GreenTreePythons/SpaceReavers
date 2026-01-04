using UnityEngine;

namespace _Scripts.Character
{   
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] float m_RotateSpeedDegPerSec = 720f;

        private CharacterController m_CharacterController;
        private PlayerAnimationController m_PlayerAnimationController;
        private Transform m_CameraTransform;
        private PlayerStateType m_CurrentPlayerLocomotionType;

        public Vector3 WorldMoveDirection { get; private set; }
        public bool IsMoving { get; private set; }

        public void Initialize(CharacterController cc, PlayerAnimationController animController)
        {
            m_CharacterController = cc;
            m_CameraTransform = Camera.main.transform;
            m_PlayerAnimationController = animController;
            IsMoving = false;
            ChangeLocomotion(false);
        }

        public void OnFixedUpdate(Vector2 input, float deltaTime, float moveSpeed)
        {
            Vector3 forward = m_CameraTransform.forward;
            forward.y = 0f;
            Vector3 right = m_CameraTransform.right;
            right.y = 0f;

            Vector3 worldDir = right * input.x + forward * input.y;
            IsMoving = worldDir.magnitude > 0f;
            if (IsMoving) 
            {
                WorldMoveDirection = worldDir;
                Quaternion targetRot = Quaternion.LookRotation(worldDir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    m_RotateSpeedDegPerSec * deltaTime
                );

                m_CharacterController.Move(worldDir * (moveSpeed * deltaTime));
            }
            ChangeLocomotion(IsMoving);
        }

        private void ChangeLocomotion(bool isMoving)
        {
            var nextLocomotionType = isMoving ? PlayerStateType.Move : PlayerStateType.Idle;
            if(m_CurrentPlayerLocomotionType == nextLocomotionType) return;
            
            m_CurrentPlayerLocomotionType = nextLocomotionType;
            m_PlayerAnimationController.PlayAnimation(m_CurrentPlayerLocomotionType);
        }
    }
}
