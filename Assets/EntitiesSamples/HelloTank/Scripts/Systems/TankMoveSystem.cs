using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    [UpdateAfter(typeof(TankSpawnSystem))]
    public partial struct TankMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TankMoveComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            new TankMoveJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        partial struct TankMoveJob : IJobEntity
        {
            public float DeltaTime;

            //private void Execute(ref LocalTransform transform, in TankMoveComponent component)
            //{
            //    transform.Position += math.up() * DeltaTime;
            //}
            private void Execute(ref TankAspect aspect, [EntityIndexInChunk] int sortKey)
            {
                aspect.Move(DeltaTime, sortKey);
            }
        }
    }
}