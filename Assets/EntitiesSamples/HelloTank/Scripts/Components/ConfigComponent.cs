using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    public partial struct ConfigComponent : IComponentData
    {
        public int TankCount;
        public Entity TankEntity;
    }
}