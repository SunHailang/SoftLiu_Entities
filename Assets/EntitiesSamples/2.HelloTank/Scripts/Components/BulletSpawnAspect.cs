using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloTank
{
    public readonly partial struct BulletSpawnAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<BulletSpawnComponent> _bulletSpawnComponent;
        private readonly RefRW<RandomComponent> _randomComponent;
        private readonly RefRO<BulletColorComponent> _bulletColorComponent;

        public Entity GetBullSpawnPrefabEntity()
        {
            return _bulletSpawnComponent.ValueRO.BulletPrefab;
        }

        public Entity GetBullSpawnPointEntity()
        {
            return _bulletSpawnComponent.ValueRO.BulletSpawnPoint;
        }

        public float GetRandomFloat()
        {
            return _randomComponent.ValueRW.Value.NextFloat();
        }

        public float4 GetBulletColor()
        {
            return _bulletColorComponent.ValueRO.BulletColor;
        }
    }
}