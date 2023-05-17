using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloCube
{
    public readonly partial struct HelloCubeAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<CubeRotationComponent> _rotationComponent;
        private readonly RefRW<LocalTransform> _localTransform;

        private float AngleValue
        {
            get => _rotationComponent.ValueRO.RotationTimer;
            set => _rotationComponent.ValueRW.RotationTimer = value;
        }


        public void Rotation(float deltaTime)
        {
            AngleValue += deltaTime;
            _localTransform.ValueRW.Rotation = quaternion.AxisAngle(math.up(), AngleValue * _rotationComponent.ValueRO.RotationSpeed);
        }
        public void Rise(float deltaTime)
        {
            _localTransform.ValueRW.Position += math.up() * _rotationComponent.ValueRO.RotationSpeed / 100 * deltaTime;
        }
    }
}