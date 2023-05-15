using Unity.Entities;

namespace EntitiesSamples.HelloCube
{
    public partial struct CubeRotationComponent : IComponentData, IEnableableComponent
    {
        public float RotationSpeed;
        public float RotationTimer;
    }
}