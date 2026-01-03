using _Datas;
using UnityEngine;

namespace _Scripts.Character
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] PlayerAnimations m_AnimationData;
        
        private Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        public void PlayAnimation(PlayerStateType state)
        {
            var clip = m_AnimationData.GetClip(state);
            m_Animator.CrossFade(clip.ClipName, clip.TransitionDuration);
        }
    }
}