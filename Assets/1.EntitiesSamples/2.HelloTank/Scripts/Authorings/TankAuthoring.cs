﻿using Unity.Entities;
using UnityEngine;


namespace EntitiesSamples.HelloTank
{
    public class TankAuthoring : MonoBehaviour
    {
        public float tankMoveSpeed;

        public uint randomSeed;

        private class Baker : Baker<TankAuthoring>
        {
            public override void Bake(TankAuthoring authoring)
            {
                var authorEntity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
                AddComponent(authorEntity, new TankMoveComponent()
                {
                    MoveSpeed = authoring.tankMoveSpeed
                });
                AddComponent(authorEntity, new RandomComponent()
                {
                    Value = Unity.Mathematics.Random.CreateFromIndex(authoring.randomSeed)
                });
            }
        }
    }
}