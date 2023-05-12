using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    public struct TankMoveComponent : IComponentData
    {
        public float MoveSpeed;
    }
}