using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloTank
{
    public readonly partial struct BulletAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _localTransform;

        private readonly RefRW<BulletMoveComponent> _bulletMoveComponent;


        public float3 Position
        {
            get => _localTransform.ValueRO.Position;
            set => _localTransform.ValueRW.Position = value;
        }
        

        public float3 Velocity
        {
            get => _bulletMoveComponent.ValueRO.Velocity;
            set => _bulletMoveComponent.ValueRW.Velocity = value;
        }

    }
}