using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloPolygon
{
    [InternalBufferCapacity(8)]
    public struct PolygonBufferComponent : IBufferElementData
    {
        public float3 Value;
    }
}