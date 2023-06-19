using Unity.Entities;
using UnityEngine;

namespace EntitiesSamples.HelloCube
{
    public class CubeRotationAuthoring : MonoBehaviour
    {
        public float rotationSpeed;

        class CubeRotationBaker : Baker<CubeRotationAuthoring>
        {
            public override void Bake(CubeRotationAuthoring authoring)
            {
                var authorEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(authorEntity, new CubeRotationComponent()
                {
                    RotationSpeed = authoring.rotationSpeed
                });
            }
        }
    }
}