using System.Collections;
using _Scripts.Projectile;
using UnityEngine;

namespace _Scripts.Character
{
    public class PlayerAttackController : MonoBehaviour
    {
        [Header("Stat")]
        [SerializeField] float m_AttackSpeed = 3f;
        
        [Header("Targeting")]
        [SerializeField] LayerMask m_TargetMask;
        [SerializeField] float m_AttackRange = 3.5f;
        [SerializeField] float m_LookRotationSpeed = 50f;
        [SerializeField] int m_CachingTargetCount = 10;

        [Header("Animation Overlay")]
        [SerializeField] private int m_AttackAnimationLayer = 1;

        [Header("Effect")]
        [SerializeField] ParticleSystem m_MuzzleVFX;
        [SerializeField] AudioClip m_FireSound;

        [Header("Projectile")]
        [SerializeField] Transform m_MuzzleRoot;
        [SerializeField] int m_ProjectileCount;
        
        public bool IsAttacking { get; private set; }
        
        private Transform m_CurrentTargetTransform;
        private PlayerAnimationController m_AnimationController;
        private ProjectilePoolContorller m_ProjectilePoolContorller;
        private Collider[] m_TargetColliders;
        
        private Coroutine m_AttackCoroutine;
        private Coroutine m_AttackAnimCoroutine;
        
        private float m_AttackCooldownTimer;

        public void Initialize(PlayerAnimationController ac, ProjectilePoolContorller pc)
        {
            m_AnimationController = ac;
            m_ProjectilePoolContorller = pc;
            m_ProjectilePoolContorller.Initialize(m_MuzzleRoot);
            m_TargetColliders = new Collider[m_CachingTargetCount];
            IsAttacking = false;
            m_AttackCooldownTimer = 0f;
        }

        public void OnFixedUpdate(float deltaTime)
        {   
            m_AttackCooldownTimer -= deltaTime;
            
            m_CurrentTargetTransform = GetTargetInRange();
            var isTargetExist = m_CurrentTargetTransform != null;
            if (!isTargetExist) return;
            
            SetTartget(deltaTime);
            
            if (m_AttackCooldownTimer > 0f) return;
            m_AttackCooldownTimer = 1f / m_AttackSpeed;
            
            if(m_AttackCoroutine != null) StopCoroutine(m_AttackCoroutine);
            m_AttackCoroutine = StartCoroutine(CoPlayFireAnimation());
        }

        private void SetTartget(float deltaTime)
        {
            var targetTransform = m_CurrentTargetTransform;
            var targetDir = targetTransform.position - transform.position;
            targetDir.y = 0f;
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,deltaTime * m_LookRotationSpeed);
        }

        private Transform GetTargetInRange()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, m_AttackRange, m_TargetColliders, m_TargetMask);
            if (count <= 0) return null;

            Transform targetTransform = null;
            float bestDistSq = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                float distance = (m_TargetColliders[i].transform.position - transform.position).sqrMagnitude;
                if (distance < bestDistSq)
                {
                    bestDistSq = distance;
                    targetTransform = m_TargetColliders[i].transform;
                }
            }

            return targetTransform;
        }
        
        private IEnumerator CoPlayFireAnimation()
        {
            float playTime = m_AnimationController.GetAnimationPlayTime(
                PlayerStateType.Attack,
                AnimationParameterType.AttackSpeed,
                m_AttackSpeed);
            
            float fireTime = playTime * 0.2f;
            
            m_AnimationController.SetLayerWeight(m_AttackAnimationLayer, 1f);
            
            if(m_AttackAnimCoroutine != null) StopCoroutine(m_AttackAnimCoroutine);
            m_AttackAnimCoroutine = StartCoroutine(m_AnimationController.CoPlayAnimation(
                PlayerStateType.Attack,
                AnimationParameterType.AttackSpeed,
                m_AttackSpeed));
            
            float elapsed = 0f;
            bool fired = false;
            while (elapsed < playTime)
            {
                elapsed += Time.deltaTime;
                if (!fired && elapsed >= fireTime)
                {
                    fired = true;
                    FireSingleShot();
                }
                float t = Mathf.Clamp01(elapsed / playTime);
                m_AnimationController.SetLayerWeight(m_AttackAnimationLayer, Mathf.Lerp(1f, 0f, t));
                yield return null;
            }
            
            m_AnimationController.SetLayerWeight(m_AttackAnimationLayer, 0f);
        }

        public void FireSingleShot()
        {
            if(m_CurrentTargetTransform == null) return;
            
            if(!m_MuzzleVFX.gameObject.activeSelf) m_MuzzleVFX.gameObject.SetActive(true);
            m_MuzzleVFX.Play();
            
            var projectile = m_ProjectilePoolContorller.GetProjectile();
            projectile.transform.position = m_MuzzleRoot.position;
            
            var direction = m_CurrentTargetTransform.position - m_MuzzleRoot.position;
            direction.y = 0f;
            projectile.Initialize(direction.normalized);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_AttackRange);
        }
#endif
    }
}
