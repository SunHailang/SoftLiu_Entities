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
        public float moveSpeed;


        class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                AddComponent(GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic), new BulletMoveComponent
                {
                    MoveSpeed = authoring.moveSpeed
                });
            }
        }
    }
}