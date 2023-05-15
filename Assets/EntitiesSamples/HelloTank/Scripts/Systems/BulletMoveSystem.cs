using Unity.Burst;
using Unity.Entities;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    [UpdateAfter(typeof(BulletSpawnSystem))]
    public partial struct BulletMoveSystem : ISystem
    {
     
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            new BulletMoveJob
            {

            }.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        private partial struct BulletMoveJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute()
            {
                
            }
        }
    }
}