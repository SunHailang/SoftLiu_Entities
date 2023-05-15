using Unity.Burst;
using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
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
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.Run();

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        private partial struct BulletSpawnJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;

            private void Execute([EntityIndexInQuery]int sortKey)
            {
                
            }
            
        }
    }
}