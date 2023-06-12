using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloTank
{
    public partial struct BulletMoveComponent : IComponentData
    {
        public float3 Velocity;
    }
}