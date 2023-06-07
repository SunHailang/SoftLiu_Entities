using PhysicsSamples.HelloWorld;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace PhysicsSamples.ChangeVelocity
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct VelocitySystem : ISystem
    {
        private ComponentLookup<PhysicsVelocity> _lookupVelocity;
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsVelocity>();

            _lookupVelocity = state.GetComponentLookup<PhysicsVelocity>();

        }

        public void OnUpdate(ref SystemState state)
        {
            _lookupVelocity.Update(ref state);
            //var lookupVelocity = state.GetComponentLookup<PhysicsVelocity>();

            foreach (var (velocity, transform) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<LocalTransform>>().WithAll<PlayerTagComponent>())
            {
                velocity.ValueRW.Angular = float3.zero;
                velocity.ValueRW.Linear = math.forward();
            }
            
            // var job = new VelocityJob
            // {
            //     DeltaTime = SystemAPI.Time.fixedDeltaTime,
            //     VelocityLookup = _lookupVelocity
            // };
            // state.Dependency = job.Schedule(state.Dependency);
        }

        private partial struct VelocityJob : IJobEntity
        {
            [ReadOnly] public float DeltaTime;
            public ComponentLookup<PhysicsVelocity> VelocityLookup;

            private void Execute(Entity entity, in PlayerTagComponent playerTag)
            {
                var random = Unity.Mathematics.Random.CreateFromIndex(123);
                if (VelocityLookup.HasComponent(entity))
                {
                    var angular = math.forward(); // / 2f;

                    var linear = angular * 0.5f;


                    // Velocitys[entity] = new PhysicsVelocity
                    // {
                    //     Linear = linear,
                    //     Angular = angular
                    // };
                }
            }
        }
    }
}