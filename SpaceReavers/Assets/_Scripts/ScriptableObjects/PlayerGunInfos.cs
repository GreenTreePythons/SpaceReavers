using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace _Datas
{
    public class PlayerGuns
    {
        public enum GunType
        {
            Rifle,
            Shotgun,
            Laser,
        }

        [CreateAssetMenu(fileName = "ShootConfiguration", menuName = "ScriptableObject/ShootConfiguration")]
        public class PlayerGunInfo : ScriptableObject
        {
            public GunType GunType;
            public string Name;
            public GameObject Prefab;
            public Vector3 SpawnPosition;
            public Vector3 SpawnRotation;

            public ShootConfiguration ShootConfig;
            public TrailConfiguration TrailConfig;

            private MonoBehaviour ActiveMono;
            private GameObject Model;
            private float LastShootTime;
            private ParticleSystem ShootSystem;
            private ObjectPool<TrailRenderer> TrailRendererObjectPool;

            public void Spawn(Transform parent, MonoBehaviour activeMono)
            {
                this.ActiveMono = activeMono;
                LastShootTime = 0f;
                // TrailRendererObjectPool = new ObjectPool<TrailRenderer>(CreateTrail);
            }

            public void Shoot()
            {
                
            }

            private TrailRenderer CreateTrail()
            {
                GameObject trail = new GameObject("BulletTrail");
                TrailRenderer trailRenderer = trail.AddComponent<TrailRenderer>();
                trailRenderer.colorGradient = TrailConfig.ColorGradient;
                trailRenderer.material = TrailConfig.Material;
                trailRenderer.widthCurve = TrailConfig.WidthCurve;
                trailRenderer.time = TrailConfig.Duration;
                trailRenderer.minVertexDistance = TrailConfig.MinVertexDistance;
                trailRenderer.emitting = false;
                trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                return trailRenderer;
            }
        }
    
        [CreateAssetMenu(fileName = "ShootConfiguration", menuName = "ScriptableObject/ShootConfiguration")]
        public class ShootConfiguration : ScriptableObject
        {
            public LayerMask LayerMask;
            public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
            public float DefaultFireRate = 0.25f;
        }

        [CreateAssetMenu(fileName = "TrailConfiguration", menuName = "ScriptableObject/TrailConfiguration")]
        public class TrailConfiguration : ScriptableObject
        {
            public Material Material;
            public AnimationCurve WidthCurve;
            public float Duration = 0.5f;
            public float MinVertexDistance = 0.1f;
            public Gradient ColorGradient;
            public float MissDistance = 100f;
            public float SimulationSpeed = 100f;
        }
    }
}