using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;


namespace EntitiesSamples.HelloTank
{
    public readonly partial struct TankAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _localTransform;

        private readonly RefRO<TankMoveComponent> _tankMoveComponent;

        private readonly RefRW<RandomComponent> _tankRandomComponent;


        public void Move(float deltaTime, int sortKey)
        {
            var pos = _localTransform.ValueRO.Position;
            float randomValue = _tankRandomComponent.ValueRW.Value.NextFloat(0.0f, 0.2f);
            var angle = (0.35f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);
            _localTransform.ValueRW.Position += dir * deltaTime * _tankMoveComponent.ValueRO.MoveSpeed;
            _localTransform.ValueRW.Rotation = quaternion.RotateY(angle);
        }
    }
}