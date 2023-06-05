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
        public void OnUpdate(ref SystemState state)
        {
            var lookupVelocity = state.GetComponentLookup<PhysicsVelocity>();

            var job = new VelocityJob
            {
                DeltaTime = SystemAPI.Time.fixedDeltaTime,
                Velocitys = lookupVelocity
            };
            state.Dependency = job.Schedule(state.Dependency);
        }

        private partial struct VelocityJob : IJobEntity
        {
            [ReadOnly] public float DeltaTime;
            public ComponentLookup<PhysicsVelocity> Velocitys;

            private void Execute(Entity entity, LocalTransform transform, in PhysicsCollider collider)
            {
                var random = Unity.Mathematics.Random.CreateFromIndex(123);
                if (Velocitys.HasComponent(entity))
                {
                    var angular = math.forward(); // / 2f;

                    var linear = float3.zero; // math.normalizesafe(angular) * 0.5f * math.length(angular);


                    Velocitys[entity] = new PhysicsVelocity
                    {
                        Linear = linear,
                        Angular = angular
                    };
                }
            }
        }
    }
}