using Unity.Entities;
using Unity.Mathematics;

namespace PhysicsSamples.HelloWorld
{
    public struct MoveComponent : IComponentData
    {
        public float3 MoveDirection;
        public float MoveSpeed;
    }
}