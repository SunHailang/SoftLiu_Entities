using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    public struct TankMoveComponent : IComponentData, IEnableableComponent
    {
        public float MoveSpeed;
    }
}