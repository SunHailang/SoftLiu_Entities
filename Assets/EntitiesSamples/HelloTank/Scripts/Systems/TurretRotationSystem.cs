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
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TurretComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            // foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<TurretComponent>())
            // {
            //     transform.ValueRW.Rotation = math.mul(quaternion.RotateY(deltaTime * math.PI), transform.ValueRO.Rotation);
            // }

            new TurretRotationJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        partial struct TurretRotationJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute(ref LocalTransform transform, in TurretComponent component)
            {
                transform.Rotation = math.mul(quaternion.RotateY(DeltaTime * component.RotationSpeed), transform.Rotation);
            }
        }
    }
}