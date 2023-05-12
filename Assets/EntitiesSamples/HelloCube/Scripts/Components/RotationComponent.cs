using Unity.Entities;

namespace EntitiesSamples.HelloCube
{
    public partial struct RotationComponent : IComponentData, IEnableableComponent
    {
        public float RotationSpeed;
        public float RotationTimer;
    }
}