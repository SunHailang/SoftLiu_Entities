using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

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
                var random = new RandomComponent
                {
                    Value = Random.CreateFromIndex(31)
                };
                AddComponent(entity, random);
                AddComponent(entity, new BulletColorComponent
                {
                    BulletColor = new float4(1, 0, 0, 1f)
                });
            }
        }
    }
}