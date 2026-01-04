using _Scripts.Projectile;
using UnityEngine;

namespace _Scripts.Character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float m_MoveSpeed = 2.0f;
        
        private PlayerInputController m_PlayerInputController;
        private PlayerStateController m_PlayerStateController;
        private PlayerMoveController m_PlayerMoveController;
        private PlayerAttackController m_PlayerAttackController;
        private PlayerAnimationController m_PlayerAnimationController;
        private ProjectilePoolContorller m_ProjectilePoolContorller;
        private CharacterController m_CharacterController;
        
        private void Awake()
        {
            m_PlayerInputController = GetComponent<PlayerInputController>();
            m_PlayerStateController = GetComponent<PlayerStateController>();
            m_PlayerMoveController = GetComponent<PlayerMoveController>();
            m_PlayerAttackController = GetComponent<PlayerAttackController>();
            m_PlayerAnimationController = GetComponent<PlayerAnimationController>();
            m_ProjectilePoolContorller = GetComponent<ProjectilePoolContorller>();
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            m_PlayerMoveController.Initialize(m_CharacterController, m_PlayerAnimationController);
            m_PlayerStateController.Initialize(m_PlayerAnimationController);
            m_PlayerAttackController.Initialize(m_PlayerAnimationController, m_ProjectilePoolContorller);
        }

        private void Update()
        {
            m_PlayerStateController.OnUpdate();
        }

        private void FixedUpdate()
        {
            m_PlayerMoveController.OnFixedUpdate(m_PlayerInputController.InputDirection, Time.fixedDeltaTime, m_MoveSpeed);
            m_PlayerAttackController.OnFixedUpdate(Time.fixedDeltaTime);
        }
    }
}