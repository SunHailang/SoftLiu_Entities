using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace EntitiesSamples.HelloCube
{
    public class CubeMoveTrackAuthoring : MonoBehaviour
    {
        public List<Transform> InitPositions;
        class CubeMoveTrackBaker : Baker<CubeMoveTrackAuthoring>
        {
            public override void Bake(CubeMoveTrackAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var buffers = AddBuffer<CubeMoveTrackComponent>(entity);
                foreach (var transform in authoring.InitPositions)
                {
                    buffers.Add(new CubeMoveTrackComponent
                    {
                        Value = transform.position
                    });
                }
            }
        }
    }
}