using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Character
{
    public enum PlayerStateType
    {
        Idle,
        Move,
        Attack,
        Reload,
        Jump
    };
    
    public class PlayerStateController : MonoBehaviour
    {
        public PlayerStateType CurrentStateType { get; private set; }
        
        private PlayerAnimationController m_PlayerAnimationController;
        private Dictionary<PlayerStateType, PlayerState> m_States = new();
        private PlayerState m_CurrentState;
        
        private void Awake()
        {
            m_PlayerAnimationController = GetComponent<PlayerAnimationController>();
            
            RegisterState(PlayerStateType.Idle, new PlayerIdleState(m_PlayerAnimationController));
            RegisterState(PlayerStateType.Move, new PlayerMoveState(m_PlayerAnimationController));
            RegisterState(PlayerStateType.Attack, new PlayerAttackState(m_PlayerAnimationController));
            
            ChangeState(PlayerStateType.Idle);
        }
        
        private void RegisterState(PlayerStateType type, PlayerState state)
        {
            m_States.Add(type, state);
        }
        
        public void ChangeState(PlayerStateType nextStateType)
        {
            if (CurrentStateType == nextStateType) return;

            m_CurrentState?.OnExit();

            CurrentStateType = nextStateType;
            m_CurrentState = m_States.GetValueOrDefault(CurrentStateType);
            m_CurrentState.OnEnter(CurrentStateType);
        }

        public void UpdateState()
        {
            m_CurrentState?.OnUpdate();
        }
    }
}