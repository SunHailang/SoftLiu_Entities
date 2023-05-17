using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloTank
{
    public readonly partial struct BulletSpawnAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<BulletSpawnComponent> _bulletSpawnComponent;

        public Entity GetBullSpawnPrefabEntity()
        {
            return _bulletSpawnComponent.ValueRO.BulletPrefab;
        }

        public Entity GetBullSpawnPointEntity()
        {
            return _bulletSpawnComponent.ValueRO.BulletSpawnPoint;
        }
    }
}