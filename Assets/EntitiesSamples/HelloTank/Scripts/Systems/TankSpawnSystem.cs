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
            var configEntity = SystemAPI.GetSingletonEntity<ConfigComponent>();
            var spawnAspect = SystemAPI.GetAspectRW<TankSpawnAspect>(configEntity);
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            for (int i = 0; i < spawnAspect.TankSpawnNumber; i++)
            {
                var newTankEntity = ecb.Instantiate(spawnAspect.TankEntityPrefab);
                var tankTansform = spawnAspect.GetRandomTankTransform();
                ecb.SetComponent(newTankEntity, new LocalToWorld {Value = tankTansform.ToMatrix()});
                ecb.SetComponent(newTankEntity, new TankMoveComponent
                {
                    MoveSpeed =  spawnAspect.GetRandomFloat()
                });
            }
            ecb.Playback(state.EntityManager);

            // var ecbSingleto = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            //
            // new SpawnTankJob
            // {
            //     DateTime = deltaTime,
            //     ECB = ecbSingleto.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            // }.ScheduleParallel();
        }

        [BurstCompile]
        partial struct SpawnTankJob : IJobEntity
        {
            public float DateTime;
            public EntityCommandBuffer.ParallelWriter ECB;
            
            private void Execute(TankSpawnAspect aspect, [EntityInQueryIndex]int sortKey)
            {
               var newTank = ECB.Instantiate(sortKey, aspect.TankEntityPrefab);
               ECB.SetComponent(sortKey, newTank, new TurretComponent
               {
                   RotationSpeed = aspect.GetRandomFloat()
               });
            }
        }
    }

    public readonly partial struct TankSpawnAspect : IAspect
    {
        // public readonly Entity Entity;
        // private readonly TransformAspect _transformAspect;

        private readonly RefRO<ConfigComponent> _configComponent;
        private readonly RefRW<RandomComponent> _randomComponent;

        public Entity TankEntityPrefab => _configComponent.ValueRO.TankEntity;

        public int TankSpawnNumber => _configComponent.ValueRO.TankCount;

        public UniformScaleTransform GetRandomTankTransform()
        {
            return new UniformScaleTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale(0.5f)
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition = float3.zero;
            float3 max = new float3(10);
            randomPosition = _randomComponent.ValueRW.Value.NextFloat3(max);
            
            return randomPosition;
        }

        private quaternion GetRandomRotation()
        {
            quaternion randomRotation = quaternion.identity;

            return randomRotation;
        }

        private float GetRandomScale(float min)
        {
            float randomScale = 1.0f;
            randomScale = _randomComponent.ValueRW.Value.NextFloat(min, 1);
            return randomScale;
        }

        public float GetRandomFloat()
        {
            return _randomComponent.ValueRW.Value.NextFloat(3f, 10f);
        }
    }
}