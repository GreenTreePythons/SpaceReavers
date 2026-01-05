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
    };
    
    public class PlayerStateController : MonoBehaviour
    {
        public PlayerStateType CurrentStateType { get; private set; }
        
        private PlayerAnimationController m_PlayerAnimationController;
        private Dictionary<PlayerStateType, PlayerState> m_States = new();
        private PlayerState m_CurrentState;

        public void Initialize(PlayerAnimationController animController)
        {
            m_PlayerAnimationController = animController;
            m_States.Add(PlayerStateType.Attack, new PlayerAttackState(m_PlayerAnimationController));
        }
        
        public void ChangeState(PlayerStateType nextStateType)
        {
            if (CurrentStateType == nextStateType) return;

            m_CurrentState?.OnExit();

            CurrentStateType = nextStateType;
            m_CurrentState = m_States.GetValueOrDefault(CurrentStateType);
            m_CurrentState.OnEnter(CurrentStateType);
        }

        public void OnUpdate() => m_CurrentState?.OnUpdate();
    }
}