using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloPolygon
{
    public partial struct PolygonAreaSystem : ISystem
    {
        private BufferLookup<PolygonBufferComponent> _buffLookup;

        public void OnCreate(ref SystemState state)
        {
            _buffLookup = state.GetBufferLookup<PolygonBufferComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _buffLookup.Update(ref state);

            
            
            var job = new PolygonAreaJob
            {

            };
            
            state.Dependency = job.ScheduleParallel(state.Dependency);
        }
        
        
        private partial struct PolygonAreaJob : IJobEntity
        {
            [ReadOnly] public float3 Position;
            
            private void Execute()
            {
                
            }
        }
        
    }

}