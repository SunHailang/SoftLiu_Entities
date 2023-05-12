using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    public partial struct BulletMoveComponent : IComponentData
    {
        public float MoveSpeed;
    }
}