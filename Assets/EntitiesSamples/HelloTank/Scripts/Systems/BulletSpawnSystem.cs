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
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            
            new BulletSpawnJob
            {
                LocalToWorldLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
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
            
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;

            private void Execute([EntityIndexInChunk]int sortKey, ref BulletSpawnAspect aspect)
            {
                var newBullet = ECB.Instantiate(sortKey, aspect.GetBullSpawnPrefabEntity());
                var spawnPoint = LocalToWorldLookup[aspect.GetBullSpawnPointEntity()];
                var spawnScale = LocalTransformLookup[aspect.GetBullSpawnPrefabEntity()].Scale;
                
                ECB.SetComponent(sortKey, newBullet, new LocalTransform
                {
                    Position = spawnPoint.Position,
                    Rotation = quaternion.identity,
                    Scale = spawnScale
                });

                ECB.SetComponent(sortKey, newBullet, new BulletMoveComponent
                {
                    Velocity = spawnPoint.Forward * 90f
                });
                ECB.SetComponent(sortKey, newBullet, new URPMaterialPropertyBaseColor
                {
                    Value = new float4(1, 0, 0, 1)
                });
            }
            
        }
    }
}