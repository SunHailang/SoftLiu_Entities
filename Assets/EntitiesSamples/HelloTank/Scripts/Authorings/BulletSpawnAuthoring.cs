
using Unity.Entities;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    public partial class BulletSpawnAuthoring : MonoBehaviour
    {
        public GameObject bulletSpawnPoint;
        public GameObject bulletPrefab;

        class Baker : Baker<BulletSpawnAuthoring>
        {
            public override void Bake(BulletSpawnAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BulletSpawnComponent
                {
                    BulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                    BulletSpawnPoint = GetEntity(authoring.bulletSpawnPoint, TransformUsageFlags.Dynamic)
                });
            }
        }
        
    }
}