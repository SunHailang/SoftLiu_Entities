using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    [UpdateAfter(typeof(TankSpawnSystem))]
    public partial struct TurretRotationSystem : ISystem
    {
        private float timer;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            new TurretRotationJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }
        
        [BurstCompile]
        partial struct TurretRotationJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute(TurretAsect asect)
            {
                asect.Rotation(DeltaTime);
            }
        }
    }

    public readonly partial struct TurretAsect : IAspect
    {
        public readonly Entity Entity;

        private readonly TransformAspect _transformAspect;
        private readonly RefRO<TurretComponent> _turretComponent;

        public void Rotation(float deltaTime)
        {
            //_transformAspect.Position += math.up() * deltaTime;
            _transformAspect.RotateWorld(quaternion.RotateY(_turretComponent.ValueRO.RotationSpeed * deltaTime * math.PI));
        }
    }
}