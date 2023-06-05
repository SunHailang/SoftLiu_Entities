using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    [UpdateAfter(typeof(BulletSpawnSystem))]
    public partial struct BulletMoveSystem : ISystem
    {

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BulletMoveComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            new BulletMoveJob
            {
                DeltaTime = deltaTime,
                ECB = ecb
            }.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        private partial struct BulletMoveChunkJob : IJobChunk
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;
            public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
            public ComponentTypeHandle<BulletMoveComponent> BulletMoveTypeHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                var gravity = new float3(0.0f, -9.82f, 0.0f);
                var invertY = new float3(1.0f, -1.0f, 1.0f);

                var localTransforms = chunk.GetNativeArray(ref TransformTypeHandle);
                var bulletMoves = chunk.GetNativeArray(ref BulletMoveTypeHandle);

                

                for (int i = 0, chunkEntityCount = chunk.Count; i < chunkEntityCount; i++)
                {
                    var localTransform = localTransforms[i];
                    localTransform.Position += bulletMoves[i].Velocity * DeltaTime;
                    if (localTransform.Position.y <= 0f)
                    {
                        localTransform.Position *= invertY;
                    }
                }
            }
        }

        [BurstCompile]
        private partial struct BulletMoveJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;


            private void Execute([EntityIndexInChunk] int sortKey, BulletAspect aspect)
            {
                var gravity = new float3(0.0f, -9.82f, 0.0f);
                var invertY = new float3(1.0f, -1.0f, 1.0f);

                aspect.Position += aspect.Velocity * DeltaTime;
                if (aspect.Position.y <= 0f)
                {
                    aspect.Position *= invertY;
                    aspect.Velocity *= invertY * 0.8f;
                }
                aspect.Velocity += gravity * DeltaTime;

                var speed = math.lengthsq(aspect.Velocity);
                if (speed < 0.1f) ECB.DestroyEntity(sortKey, aspect.Entity);
            }
        }
    }
}