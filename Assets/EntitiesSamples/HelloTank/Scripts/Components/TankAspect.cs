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

        private readonly TransformAspect _transformAspect;

        private readonly RefRO<TankMoveComponent> _tankMoveComponent;

        private readonly RefRW<RandomComponent> _tankRandomComponent;


        public void Move(float deltaTime)
        {
            var pos = _transformAspect.Position;
            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);
            _transformAspect.Position += dir * deltaTime * _tankMoveComponent.ValueRO.MoveSpeed;
            _transformAspect.Rotation = quaternion.RotateY(angle);
        }
    }
}