using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    public partial struct TankSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ConfigComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            //var config = SystemAPI.GetSingleton<ConfigComponent>();

            //var ecb = new EntityCommandBuffer(Allocator.Temp);
            //for (int i = 0; i < config.TankCount; i++)
            //{
            //    ecb.Instantiate(config.TankEntity);
            //}
            //ecb.Playback(state.EntityManager);

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            new TankSpawnJob
            {
                Ecb = ecb.AsParallelWriter()
            }.ScheduleParallel();
        }

        private partial struct TankSpawnJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter Ecb;

            private void Execute(TankSpawnAspect aspect, in ConfigComponent config, [EntityIndexInChunk] int sortKey)
            {
                for (var i = 0; i < config.TankCount; i++)
                {
                    var newTank = Ecb.Instantiate(sortKey, aspect.TankPrefabEntity);
                    Ecb.SetComponent(sortKey, newTank, aspect.GetRandomTransform());
                }
            }
        }
    }

    public readonly partial struct TankSpawnAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<ConfigComponent> _configComponent;

        private readonly RefRW<RandomComponent> _randomComponent;

        public Entity TankPrefabEntity => _configComponent.ValueRO.TankEntity;

        public LocalTransform GetRandomTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = 1f
            };
        }

        private static float3 MaxPosition => new float3(1, 0, 1) * 20;

        private float3 GetRandomPosition()
        {
            return _randomComponent.ValueRW.Value.NextFloat3(MaxPosition);
        }

        private static quaternion GetRandomRotation()
        {
            return quaternion.identity;
        }
    }
}
 