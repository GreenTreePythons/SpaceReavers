using UnityEngine;

namespace _Scripts.Projectile
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] Transform m_MuzzleRoot;
        [SerializeField] float m_ProjectileSpeed = 10f;
        [SerializeField] float m_LifeTime = 2f;
        
        [SerializeField] ParticleSystem m_FireParticle;
        [SerializeField] AudioClip m_FireSound;

        private Vector3 m_Direction;

        public void Initialize(Vector3 direction)
        {
            m_Direction = direction;
        }
        
        public void Update()
        {
            transform.position += m_Direction * m_ProjectileSpeed * Time.deltaTime;
        }
    }
}