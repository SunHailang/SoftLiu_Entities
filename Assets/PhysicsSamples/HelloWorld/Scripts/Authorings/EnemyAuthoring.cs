using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace PhysicsSamples.HelloWorld
{
    public class EnemyAuthoring : MonoBehaviour
    {
        private class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EnemyTagComponent>(entity);

                AddComponent(entity, new URPMaterialPropertyBaseColor
                {
                    Value = new float4(1.0f, 0, 0, 1.0f)
                });

                AddSharedComponent(entity, new EntitySharedComponent
                {
                    EntityType = 2
                });
            }
        }
    }
}