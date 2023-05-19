using Unity.Entities;
using Unity.Mathematics;

namespace PhysicsSamples.HelloWorld
{
    public struct RotationComponent : IComponentData
    {
        public float3 RotationDir;
        public float RotationSpeed;
    }
}