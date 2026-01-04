using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Projectile
{
    public class ProjectilePoolContorller : MonoBehaviour
    {
        [SerializeField] BaseProjectile m_Projectile;
        
        private Queue<BaseProjectile> m_ProjectileQueue = new();
        private Transform m_MuzzleRoot;

        public void Initialize(Transform muzzleRoot)
        {
            m_MuzzleRoot = muzzleRoot;
        }

        public BaseProjectile GetProjectile()
        {
            if (m_ProjectileQueue.Count > 0)
            {
                var projectile = m_ProjectileQueue.Dequeue();
                projectile.gameObject.SetActive(true);
                return projectile;
            }
            return Instantiate(m_Projectile);
        }

        private void ReturnProjectile(BaseProjectile projectile)
        {
            projectile.gameObject.SetActive(false);
            m_ProjectileQueue.Enqueue(projectile);
        }
    }
}