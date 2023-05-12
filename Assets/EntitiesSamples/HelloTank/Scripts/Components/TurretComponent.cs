using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    public partial struct TurretComponent : IComponentData
    {
        public float RotationSpeed;
    }
}