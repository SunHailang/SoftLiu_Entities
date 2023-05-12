using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace EntitiesSamples.HelloCube
{
    [BurstCompile]
    public partial struct CubeRotationSystem : ISystem
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
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (aspect, rotation) in SystemAPI.Query<TransformAspect, RefRO<RotationComponent>>())
            {
                aspect.RotateWorld(quaternion.RotateY(deltaTime * rotation.ValueRO.RotationSpeed));
            }
            // new RotationJob
            // {
            //     DeltaTime = deltaTime
            // }.ScheduleParallel();
        }


        // partial struct RotationJob : IJobEntity
        // {
        //     public float DeltaTime;
        //
        //     private void Execute(HelloCubeAspect cubeAspect)
        //     {
        //         cubeAspect.Rotation(DeltaTime);
        //         cubeAspect.Rise(DeltaTime);
        //     }
        // }
    }
}