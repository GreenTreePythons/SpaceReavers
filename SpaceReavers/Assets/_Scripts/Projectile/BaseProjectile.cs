using UnityEngine;

namespace _Scripts.Projectile
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] float m_ProjectileSpeed = 10f;
        [SerializeField] float m_LifeTime = 2f;

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