using UnityEngine;

namespace _Scripts.Character
{
    public abstract class PlayerState
    {
        protected PlayerAnimationController m_PlayerAnimationController;
        
        public PlayerState(PlayerAnimationController playerAnimationController)
        {
            m_PlayerAnimationController = playerAnimationController;
        }
        
        public virtual void OnEnter(PlayerStateType stateType)
        {
            Debug.Log($"PlayerState.OnEnter: {stateType}");
            m_PlayerAnimationController.PlayAnimation(stateType);
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnExit()
        {
            
        }
    }

    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerAnimationController playerAnimationController) : base(playerAnimationController)
        {
        }

        public override void OnEnter(PlayerStateType stateType)
        {
            base.OnEnter(stateType);
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }

    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(PlayerAnimationController playerAnimationController) : base(playerAnimationController)
        {
        }

        public override void OnEnter(PlayerStateType stateType)
        {
            base.OnEnter(stateType);
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }

    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(PlayerAnimationController playerAnimationController) : base(playerAnimationController)
        {
        }
        
        public override void OnEnter(PlayerStateType stateType)
        {
            base.OnEnter(stateType);
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}