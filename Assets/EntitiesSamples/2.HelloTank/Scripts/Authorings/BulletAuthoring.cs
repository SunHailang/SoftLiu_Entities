using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    public class BulletAuthoring : MonoBehaviour
    {

        class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BulletMoveComponent>(entity);
            }
        }
    }
}