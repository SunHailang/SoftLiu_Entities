using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    public partial struct BulletSpawnComponent : IComponentData
    {
        public Entity BulletSpawnPoint;
        public Entity BulletPrefab;
    }
}