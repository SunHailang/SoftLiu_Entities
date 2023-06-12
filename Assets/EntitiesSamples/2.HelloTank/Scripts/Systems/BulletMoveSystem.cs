using System;
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
        private BeginSimulationEntityCommandBufferSystem.Singleton _ecbSingleton;

        private ComponentTypeHandle<LocalTransform> _transformHandle;
        private ComponentTypeHandle<BulletMoveComponent> _bulletMoveHandle;
        private EntityTypeHandle _entityTypeHandle;
        
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<BulletMoveComponent>();


            _transformHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>();
            _bulletMoveHandle = SystemAPI.GetComponentTypeHandle<BulletMoveComponent>();
            _entityTypeHandle = SystemAPI.GetEntityTypeHandle();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            _transformHandle.Update(ref state);
            _bulletMoveHandle.Update(ref state);
            _entityTypeHandle.Update(ref state);

            var ret = SystemAPI.TryGetSingleton(out _ecbSingleton);
            // if (ret)
            //{
            var ecb = _ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            //}
            // new BulletMoveJob
            // {
            //     DeltaTime = deltaTime,
            //     ECB = ecb
            // }.ScheduleParallel();

            var entityQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, BulletMoveComponent>().Build();

            var job = new BulletMoveChunkJob
            {
                DeltaTime = deltaTime,
                ECB = ecb,
                TransformTypeHandle = _transformHandle,
                BulletMoveTypeHandle = _bulletMoveHandle,
                EntityTypeHandle = _entityTypeHandle
            };
            state.Dependency = job.ScheduleParallel(entityQuery, state.Dependency);
        }

        [BurstCompile]
        private struct BulletMoveChunkJob : IJobChunk
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;
            public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
            public ComponentTypeHandle<BulletMoveComponent> BulletMoveTypeHandle;
            public EntityTypeHandle EntityTypeHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                var gravity = new float3(0.0f, -9.82f, 0.0f);
                var invertY = new float3(1.0f, -1.0f, 1.0f);

                var localTransforms = chunk.GetNativeArray(ref TransformTypeHandle);
                var bulletMoves = chunk.GetNativeArray(ref BulletMoveTypeHandle);

                var entitys = chunk.GetNativeArray(EntityTypeHandle);
                
                var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
                
                while (enumerator.NextEntityIndex(out var index))
                {
                    var transform = localTransforms[index];
                    var bullet = bulletMoves[index];
                    transform.Position += bullet.Velocity * DeltaTime;
                    if (transform.Position.y <= 0)
                    {
                        transform.Position *= invertY;
                        bullet.Velocity *= invertY * 0.8f;
                    }
                    bullet.Velocity += gravity * DeltaTime;

                    localTransforms[index] = transform;
                    bulletMoves[index] = bullet;
                    
                    var speed = math.lengthsq(bulletMoves[index].Velocity);
                    if (speed < 0.1f)
                    {
                        ECB.DestroyEntity(index, entitys[index]);
                    }
                }
            }
        }

        [BurstCompile]
        private partial struct BulletMoveJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;


            private void Execute(Entity entity, [EntityIndexInChunk] int sortKey, BulletAspect aspect)
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
                if (speed < 0.1f) ECB.DestroyEntity(sortKey, entity);
            }
        }
    }
}