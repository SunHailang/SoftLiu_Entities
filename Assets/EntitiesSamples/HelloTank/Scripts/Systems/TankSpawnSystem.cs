using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    public partial struct TankSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
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
            var deltaTime = SystemAPI.Time.DeltaTime;

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
                DeltaTime = deltaTime,
                ECB = ecb.AsParallelWriter()
            }.ScheduleParallel();
        }

        private partial struct TankSpawnJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;
            private void Execute(ref TankSpawnAspect aspect, in ConfigComponent config, [EntityIndexInChunk] int sortKey)
            {
                for (int i = 0; i < config.TankCount; i++)
                {
                    var newTank = ECB.Instantiate(sortKey, aspect.TankPrefabEntity);
                    ECB.SetComponent(sortKey, newTank, aspect.GetRandomTransform());
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

        private float3 MaxPosition => new float3(1, 0, 1) * 20;

        private float3 GetRandomPosition()
        {
            return _randomComponent.ValueRW.Value.NextFloat3(MaxPosition);
        }

        private quaternion GetRandomRotation()
        {
            return quaternion.identity;
        }
    }
}