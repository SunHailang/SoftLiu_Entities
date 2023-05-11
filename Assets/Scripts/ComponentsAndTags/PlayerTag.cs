using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SoftLiu.Player
{
    public struct PlayerTag
    {

    }

    public struct PlayerMoveComponent : IComponentData
    {
        public float MoveSpeed;
        public float3 MoveDir;
    }
}
