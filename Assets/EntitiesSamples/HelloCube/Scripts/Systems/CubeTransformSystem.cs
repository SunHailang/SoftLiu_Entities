using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesSamples.HelloCube
{
    public partial struct CubeTransformSystem : ISystem
    {
        private ComponentTypeHandle<LocalTransform> _localTrans;

        public void OnCreate(ref SystemState state)
        {
            _localTrans = state.GetComponentTypeHandle<LocalTransform>(false);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _localTrans.Update(ref state);

            // new EntityJob
            // {
            //     DateTime = SystemAPI.Time.DeltaTime
            // }.ScheduleParallel();


            // var rotationQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform>().Build();
            // var job = new ChunkJob
            // {
            //     DateTime = SystemAPI.Time.DeltaTime,
            //     LocalHandle = _localTrans
            // };
            // state.Dependency = job.ScheduleParallel(rotationQuery, state.Dependency);
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