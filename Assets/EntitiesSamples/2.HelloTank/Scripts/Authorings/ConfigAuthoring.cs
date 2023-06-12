using Unity.Entities;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    public class ConfigAuthoring : MonoBehaviour
    {
        public int tankCount;
        public uint tankRandomSeed;
        public GameObject tankPrefab;

        private class Baker : Baker<ConfigAuthoring>
        {
            public override void Bake(ConfigAuthoring authoring)
            {
                var authorEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(authorEntity, new ConfigComponent
                {
                    TankCount = authoring.tankCount,
                    TankEntity = GetEntity(authoring.tankPrefab, TransformUsageFlags.Dynamic)
                });
                AddComponent(authorEntity, new RandomComponent
                {
                    Value = Unity.Mathematics.Random.CreateFromIndex(authoring.tankRandomSeed)
                });
            }
        }
    }
}