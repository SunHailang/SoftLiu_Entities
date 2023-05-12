using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloCube
{
    public readonly partial struct HelloCubeAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;

        private readonly RefRW<RotationComponent> _rotationComponent;

        private float AngleValue
        {
            get => _rotationComponent.ValueRO.RotationTimer;
            set => _rotationComponent.ValueRW.RotationTimer = value;
        }


        public void Rotation(float deltaTime)
        {
            _transformAspect.RotateWorld(quaternion.RotateY(deltaTime * _rotationComponent.ValueRO.RotationSpeed));
            // AngleValue += deltaTime;
            // _transformAspect.Rotation = quaternion.AxisAngle(math.up(), AngleValue * _rotationComponent.ValueRO.RotationSpeed);
        }
        public void Rise(float deltaTime)
        {
            _transformAspect.Position += math.up() * _rotationComponent.ValueRO.RotationSpeed / 100 * deltaTime;
        }
    }
}