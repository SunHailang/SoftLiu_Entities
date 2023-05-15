using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloCube
{
    [BurstCompile]
    public partial struct CubeRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeRotationComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            // foreach (var (transform, rotation) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CubeRotationComponent>>())
            // {
            //     // Rotate 180 degrees around Y every second.
            //     var spin = quaternion.RotateY(deltaTime * math.PI * rotation.ValueRO.RotationSpeed);
            //     transform.ValueRW.Rotation = math.mul(spin, transform.ValueRO.Rotation);
            // }

            var job = new RotationJob
            {
                DeltaTime = deltaTime
            };
            job.ScheduleParallel();
        }


        partial struct RotationJob : IJobEntity
        {
            public float DeltaTime;

            //private void Execute(ref LocalTransform transform, in CubeRotationComponent component)
            //{
            //    // Rotate 180 degrees around Y every second.
            //    //var spin = quaternion.RotateY(DeltaTime * math.PI * component.RotationSpeed);
            //    //transform.Rotation = math.mul(spin, transform.Rotation);
            //    transform.Position += math.up() * DeltaTime;
            //}

            private void Execute(ref HelloCubeAspect aspect)
            {
                aspect.Rotation(DeltaTime);
            }
        }
    }
}