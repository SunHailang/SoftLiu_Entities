using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloTank
{
    public readonly partial struct TurretAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRO<TurretComponent> _turretComponent;

        public void Rotation(float deltaTime)
        {
            // float angle = deltaTime * math.PI * _turretComponent.ValueRO.RotationSpeed;
            // _localTransform.ValueRW.Rotation = math.mul(quaternion.AxisAngle(math.up(), angle), _localTransform.ValueRO.Rotation);
            _localTransform.ValueRW.Rotation = math.mul(quaternion.RotateY(deltaTime * math.PI * _turretComponent.ValueRO.RotationSpeed), _localTransform.ValueRO.Rotation);
        }
    }
}