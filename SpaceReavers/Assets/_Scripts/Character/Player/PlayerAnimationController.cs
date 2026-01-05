using System;
using System.Collections.Generic;
using System.Collections;
using _Datas;
using UnityEngine;

namespace _Scripts.Character
{
    public enum AnimationParameterType { MoveSpeed, AttackSpeed }
     
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] PlayerAnimations m_AnimationData;
        
        private Animator m_Animator;
        private readonly Dictionary<AnimationParameterType, int> m_ParameterHashs = new();

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            InitializeParameterHashes();
        }

        private void InitializeParameterHashes()
        {
            var animatorParamHashes = new HashSet<int>();
            foreach (var parameter in m_Animator.parameters)
            {
                animatorParamHashes.Add(parameter.nameHash);
            }
            
            foreach (AnimationParameterType type in Enum.GetValues(typeof(AnimationParameterType)))
            {
                int hash = Animator.StringToHash(type.ToString());
                if (!animatorParamHashes.Contains(hash))
                {
                    Debug.LogError($"{type} is not a valid parameter");
                    continue;
                }
                m_ParameterHashs[type] = hash;
            }
        }

        public void PlayAnimation(PlayerStateType state)
        {
            var clip = m_AnimationData.GetClip(state);
            m_Animator.CrossFade(clip.ClipName, clip.TransitionDuration);
        }

        public IEnumerator CoPlayAnimation(PlayerStateType state, AnimationParameterType parameterType, float animationSpeed)
        {
            var clip = m_AnimationData.GetClip(state);
            var parameterName = m_ParameterHashs[parameterType];
            
            m_Animator.SetFloat(parameterName, animationSpeed);
            m_Animator.CrossFade(clip.ClipName, clip.TransitionDuration);
            
            float playTime = clip.Clip.length / animationSpeed;
            yield return new WaitForSeconds(playTime);
        }

        public float GetAnimationPlayTime(PlayerStateType state, AnimationParameterType parameterType, float animationSpeed)
        {
            var clip = m_AnimationData.GetClip(state);
            return clip.Clip.length / animationSpeed;
        }

        public void SetLayerWeight(int layerIndex, float weight) 
            => m_Animator.SetLayerWeight(layerIndex, weight);
    }
}