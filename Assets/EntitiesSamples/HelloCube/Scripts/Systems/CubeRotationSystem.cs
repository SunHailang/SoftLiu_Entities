using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloCube
{
    [BurstCompile]
    public partial struct CubeRotationSystem : ISystem
    {
        private ComponentTypeHandle<LocalTransform> _TransformTypeHandle;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _TransformTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
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

            // 第一种方式
            // foreach (var (transform, rotation) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CubeRotationComponent>>())
            // {
            //     // Rotate 180 degrees around Y every second.
            //     var spin = quaternion.RotateY(deltaTime * math.PI * rotation.ValueRO.RotationSpeed);
            //     transform.ValueRW.Rotation = math.mul(spin, transform.ValueRO.Rotation);
            // }

            // 第二种方式
            //var job = new RotationJob
            //{
            //    DeltaTime = deltaTime
            //};
            //job.ScheduleParallel();

            // 第三种方式
            var rotationQuery = SystemAPI.QueryBuilder().WithAll<CubeRotationComponent, LocalTransform>().Build();
            _TransformTypeHandle.Update(ref state);

            var rotationTypeHandle = SystemAPI.GetComponentTypeHandle<CubeRotationComponent>(true);

            var job = new RotationChunkJob
            {
                DeltaTime = deltaTime,
                TransformTypeHandle = _TransformTypeHandle,
                CubeRotationTypeHandle = rotationTypeHandle
            };
            state.Dependency = job.ScheduleParallel(rotationQuery, state.Dependency);
        }

        [BurstCompile]
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
            private void Execute(){
            }
            // private void Execute(ref HelloCubeAspect aspect)
            // {
            //     aspect.Rotation(DeltaTime);
            // }
        }

        [BurstCompile]
        private partial struct RotationChunkJob : IJobChunk
        {
            public float DeltaTime;
            public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
            [ReadOnly] public ComponentTypeHandle<CubeRotationComponent> CubeRotationTypeHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                var chunkTransform = chunk.GetNativeArray(ref TransformTypeHandle);
                var chunkRotation = chunk.GetNativeArray(ref CubeRotationTypeHandle);
                for (int i = 0; i < chunk.Count; i++)
                {
                    var rotationSpeed = chunkRotation[i];
                    chunkTransform[i] = chunkTransform[i].RotateY(rotationSpeed.RotationSpeed * DeltaTime);
                }
            }
        }
    }
}