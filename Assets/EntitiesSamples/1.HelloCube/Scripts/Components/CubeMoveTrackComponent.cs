using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloCube
{
    [InternalBufferCapacity(8)]
    public struct CubeMoveTrackComponent : IBufferElementData
    {
        public float3 Value;
    }
}