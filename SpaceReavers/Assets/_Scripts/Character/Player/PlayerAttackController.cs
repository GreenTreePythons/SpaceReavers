using _Scripts.Projectile;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Character
{
    public class PlayerAttackController : MonoBehaviour
    {
        [Header("Targeting")]
        [SerializeField] LayerMask m_TargetMask;
        [SerializeField] float m_AttackRange = 3.5f;
        [SerializeField] float m_LookRotationSpeed = 50f;
        [SerializeField] int m_CachingTargetCount = 10;

        [Header("Animation Overlay")]
        [SerializeField] private int m_AttackAnimationLayer = 1; // UpperBody layer index

        [Header("VFX")]
        [SerializeField] ParticleSystem m_MuzzleVFX;

        [Header("Projectile")]
        [SerializeField] Transform m_MuzzleRoot;
        [SerializeField] int m_ProjectileCount;
        
        public bool IsAttacking { get; private set; }
        
        private Transform m_CurrentTargetTransform;
        private PlayerAnimationController m_AnimationController;
        private ProjectilePoolContorller m_ProjectilePoolContorller;
        private Collider[] m_TargetColliders;
        
        private float m_AttackCooldown;

        public void Initialize(PlayerAnimationController ac, ProjectilePoolContorller pc)
        {
            m_AnimationController = ac;
            m_ProjectilePoolContorller = pc;
            m_ProjectilePoolContorller.Initialize(m_MuzzleRoot);
            m_TargetColliders = new Collider[m_CachingTargetCount];
            IsAttacking = false;
        }

        public void OnFixedUpdate(float deltaTime)
        {   
            m_CurrentTargetTransform = GetTargetInRange();
            var isTargetExist = m_CurrentTargetTransform != null;
            ChangeAttackmotion(isTargetExist);
            
            if (!isTargetExist) return;
            
            SetTartget(deltaTime);
        }

        private void SetTartget(float deltaTime)
        {
            var targetDir = m_CurrentTargetTransform.position - transform.position;
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

        private void ChangeAttackmotion(bool isAttacking)
        {
            if (IsAttacking == isAttacking) return;
            IsAttacking = isAttacking;
            if (isAttacking)
            {
                m_MuzzleVFX.Play();
                m_AnimationController.PlayAnimation(PlayerStateType.Attack);
                m_AnimationController.SetLayerWeight(AnimationLayer.Upper, 1f);
                
                var projectile = m_ProjectilePoolContorller.GetProjectile();
                projectile.transform.position = m_MuzzleRoot.position;
                var direction = m_CurrentTargetTransform.position - m_MuzzleRoot.position;
                projectile.Initialize(direction.normalized);
            }
            else
            {
                m_MuzzleVFX.Stop();
                m_AnimationController.SetLayerWeight(AnimationLayer.Upper, 0f);
            } 
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
