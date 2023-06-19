using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EntitiesSamples.HelloCube
{
    public partial struct CubeTransformSystem : ISystem
    {
        // private ComponentTypeHandle<LocalTransform> _localTransTypeHandle;
        // private BufferTypeHandle<CubeMoveTrackComponent> _moveBufferTypeHandle;
        //
        // private BufferLookup<CubeMoveTrackComponent> _moveBuffLookup;

        private BeginSimulationEntityCommandBufferSystem.Singleton _ecbSingleton;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            // _localTransTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
            // _moveBufferTypeHandle = state.GetBufferTypeHandle<CubeMoveTrackComponent>();
            //
            // _moveBuffLookup = state.GetBufferLookup<CubeMoveTrackComponent>();
        }

        // [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // _localTransTypeHandle.Update(ref state);
            // _moveBufferTypeHandle.Update(ref state);
            //
            // _moveBuffLookup.Update(ref state);
            
             var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            // var cubeMoveBuffer = SystemAPI.GetSingletonBuffer<CubeMoveTrackComponent>();

            // var job = new EntityBufferJob
            // {
            //     DeltaTime = SystemAPI.Time.DeltaTime,
            //     //MoveTrackLookup = _moveBuffLookup,
            //     CubeMoveDynamicBuffer = cubeMoveBuffer,
            //     ECB = ecb
            // };
            // state.Dependency = job.ScheduleParallel(state.Dependency);

            // new EntityJob
            // {
            //     DateTime = SystemAPI.Time.DeltaTime
            // }.ScheduleParallel();


            // var rotationQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform>().Build();
            // var chunkJob = new ChunkJob
            // {
            //     DateTime = SystemAPI.Time.DeltaTime,
            //     LocalHandle = _localTransTypeHandle
            // };
            // state.Dependency = chunkJob.ScheduleParallel(rotationQuery, state.Dependency);
        }

        private partial struct EntityBufferJob : IJobEntity
        {
            [ReadOnly] public float DeltaTime;
            //[ReadOnly] public BufferLookup<CubeMoveTrackComponent> MoveTrackLookup;
        
            [ReadOnly] public DynamicBuffer<CubeMoveTrackComponent> CubeMoveDynamicBuffer;
        
            public EntityCommandBuffer.ParallelWriter ECB;
        
            private void Execute(Entity entity, LocalTransform transform, CubeRotationComponent component)
            {
                // foreach (var trackComponent in CubeMoveDynamicBuffer)
                // {
                // }
        
                // if (MoveTrackLookup.TryGetBuffer(entity, out var bufferData))
                // {
                //     foreach (var moveTrackComponent in bufferData)
                //     {
                //         var pos= moveTrackComponent.Value;
                //         Debug.Log(pos);
                //     }
                // }
            }
        }

        private partial struct EntityJob : IJobEntity
        {
            public float DateTime;

            private void Execute(ref LocalTransform transform)
            {
                transform.Position += math.up() * DateTime;
            }
        }

        private struct ChunkJob : IJobChunk
        {
            [ReadOnly] public float DateTime;
            public ComponentTypeHandle<LocalTransform> LocalHandle;

            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                var locals = chunk.GetNativeArray(ref LocalHandle);
                for (int i = 0; i < chunk.Count; i++)
                {
                    locals[i] = locals[i].Translate(math.up() * DateTime);
                }
            }
        }
    }
}