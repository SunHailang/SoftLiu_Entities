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
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            new TankMoveJob()
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }
        
        partial struct TankMoveJob : IJobEntity
        {
            public float DeltaTime;
            private void Execute(TankAspect aspect)
            {
                aspect.Move(DeltaTime);
            }
        }
    }
}