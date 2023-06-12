using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    [UpdateAfter(typeof(TankSpawnSystem))]
    public partial struct BulletSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // var deltaTime = SystemAPI.Time.DeltaTime;

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            
            new BulletSpawnJob
            {
                LocalToWorldLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
                // DeltaTime = deltaTime,
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        private partial struct BulletSpawnJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<LocalToWorld> LocalToWorldLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            
            // public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter Ecb;

            private void Execute([EntityIndexInChunk]int sortKey, BulletSpawnAspect aspect)
            {
                var newBullet = Ecb.Instantiate(sortKey, aspect.GetBullSpawnPrefabEntity());
                var spawnPoint = LocalToWorldLookup[aspect.GetBullSpawnPointEntity()];
                var spawnScale = LocalTransformLookup[aspect.GetBullSpawnPrefabEntity()].Scale;

                var color = aspect.GetBulletColor();
                
                Ecb.SetComponent(sortKey, newBullet, new LocalTransform
                {
                    Position = spawnPoint.Position,
                    Rotation = quaternion.identity,
                    Scale = spawnScale
                });

                Ecb.SetComponent(sortKey, newBullet, new BulletMoveComponent
                {
                    Velocity = spawnPoint.Forward * 50f
                });
                Ecb.SetComponent(sortKey, newBullet, new URPMaterialPropertyBaseColor
                {
                    Value = color
                });
            }
        }
    }
}