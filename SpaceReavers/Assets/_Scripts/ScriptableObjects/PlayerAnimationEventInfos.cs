using System;
using _Scripts.Character;
using UnityEngine;

namespace _Datas
{
    public enum PlayerAnimationType
    {
        SingleShot
    }
    
    [CreateAssetMenu(fileName = "PlayerAnimationEventInfos", menuName = "ScriptableObject/PlayerAnimationEventInfos")]  
    public class PlayerAnimationEventInfos : ScriptableObject
    {
        [SerializeField] PlayerAnimationEventInfo[] m_PlayerAnimationEventInfos;
    }

    [Serializable]
    public struct PlayerAnimationEventInfo
    {
        [SerializeField] PlayerAnimationType m_PlayerAnimationType;
        [SerializeField] float m_EventTime;
        [SerializeField] PlayerAnimationEventVFXInfo[] m_VFXs;
        [SerializeField] AudioClip[] m_Audio;
    }

    [Serializable]
    public struct PlayerAnimationEventVFXInfo
    {
        [SerializeField] ParticleSystem m_VFX;
        [SerializeField] Transform m_VFXTransform;
    }
}