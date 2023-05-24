using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;

namespace PhysicsSamples.HelloWorld
{
    public struct IgnoreTransparentClosestHitCollector : ICollector<RaycastHit>
    {
        public bool EarlyOutOnFirstHit => false;

        public float MaxFraction { get; private set; }

        public int NumHits { get; private set; }

        public RaycastHit ClosestHit;

        private CollisionWorld m_World;
        private const int k_TransparentCustomTag = (1 << 1);

        public IgnoreTransparentClosestHitCollector(CollisionWorld world)
        {
            m_World = world;

            MaxFraction = 1.0f;
            ClosestHit = default;
            NumHits = 0;
        }

        private static bool IsTransparent(BlobAssetReference<Collider> collider, ColliderKey key)
        {
            bool bIsTransparent = false;
            unsafe
            {
                // Only Convex Colliders have Materials associated with them. So base on CollisionType
                // we'll need to cast from the base Collider type, hence, we need the pointer.
                var c = collider.AsPtr();
                {
                    var cc = ((ConvexCollider*) c);

                    // We also need to check if our Collider is Composite (i.e. has children).
                    // If it is then we grab the actual leaf node hit by the ray.
                    // Checking if our collider is composite
                    if (c->CollisionType != CollisionType.Convex)
                    {
                        // If it is, get the leaf as a Convex Collider
                        c->GetLeaf(key, out ChildCollider child);
                        cc = (ConvexCollider*) child.Collider;
                    }

                    // Now we've definitely got a ConvexCollider so can check the Material.
                    bIsTransparent = (cc->Material.CustomTags & k_TransparentCustomTag) != 0;
                }
            }

            return bIsTransparent;
        }

        public bool AddHit(RaycastHit hit)
        {
            if (IsTransparent(m_World.Bodies[hit.RigidBodyIndex].Collider, hit.ColliderKey))
            {
                return false;
            }

            MaxFraction = hit.Fraction;
            ClosestHit = hit;
            NumHits = 1;

            return true;
        }
    }

    public struct ComponentHandles
    {
        public ComponentLookup<LocalTransform> LocalTransforms;
        public ComponentLookup<PostTransformMatrix> PostTransformMatrices;

        public ComponentHandles(ref SystemState state)
        {
            LocalTransforms = state.GetComponentLookup<LocalTransform>(false);
            PostTransformMatrices = state.GetComponentLookup<PostTransformMatrix>(false);
        }

        public void Update(ref SystemState state)
        {
            LocalTransforms.Update(ref state);
            PostTransformMatrices.Update(ref state);
        }
    }

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct RaycastSystem : ISystem
    {
        private ComponentHandles _handle;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<PlayerTagComponent>();

            _handle = new ComponentHandles(ref state);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _handle.Update(ref state);

            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var world = physicsWorldSingleton.CollisionWorld;

            var playerEntity = SystemAPI.GetSingletonEntity<PlayerTagComponent>();
            var playerTransform = SystemAPI.GetComponentRW<LocalTransform>(playerEntity, true);

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            var job = new RaycastJob
            {
                PlayerPosition = playerTransform.ValueRO.Position,
                MaxDistance = 10f,
                Ecb = ecb,
                // LocalTransforms = _handle.LocalTransforms,
                // PostTransformMatrices = _handle.PostTransformMatrices,
                PhysicsWorldSingleton = physicsWorldSingleton
            };
            state.Dependency = job.Schedule(state.Dependency);
        }

        [BurstCompile]
        private partial struct RaycastJob : IJobEntity
        {
            [Unity.Collections.ReadOnly]public float3 PlayerPosition;
            [Unity.Collections.ReadOnly]public float MaxDistance;

            public EntityCommandBuffer.ParallelWriter Ecb;

            [Unity.Collections.ReadOnly] public PhysicsWorldSingleton PhysicsWorldSingleton;

            private void Execute([EntityIndexInChunk] int sortKey, ref LocalTransform transform, in EntitySharedComponent sharedComponent)
            {
                var rayLocalTransform = transform;

                var dirNormalize = math.normalizesafe(rayLocalTransform.Position - PlayerPosition);

                var filter = new CollisionFilter
                {
                    BelongsTo = 0xfffffff7,
                    CollidesWith = 0xfffffff7,
                    GroupIndex = 0
                };
                
                var raycastInput = new RaycastInput
                {
                    Start = PlayerPosition,
                    End = PlayerPosition + dirNormalize * MaxDistance,
                    Filter = filter
                };
                
                //var collector = new IgnoreTransparentClosestHitCollector(PhysicsWorldSingleton.CollisionWorld);
                if (sharedComponent.EntityType != 1 && PhysicsWorldSingleton.CastRay(raycastInput, out var hit))
                {
                    //var hit = collector.ClosestHit;
                    var hitDistance = MaxDistance * hit.Fraction;
                    var hitEntity = hit.Entity;
                    var hitPosition = hit.Position;
                    
                    
                    Ecb.AddComponent(sortKey, hit.Entity, new URPMaterialPropertyBaseColor
                    {
                        Value = new float4(0,  hit.Fraction, 0, 1f)
                    });
                }
            }
        }
    }
}