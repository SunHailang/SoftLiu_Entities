using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloTank
{
    public partial struct BulletColorComponent : IComponentData
    {
        public float4 BulletColor;
    }
}