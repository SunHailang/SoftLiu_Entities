using Unity.Entities;
using UnityEngine;

namespace EntitiesSamples.HelloCube
{
    public class CubeRotationAuthoring : MonoBehaviour
    {
        public float rotationSpeed;
        
        class Baker: Baker<CubeRotationAuthoring>
        {
            public override void Bake(CubeRotationAuthoring authoring)
            {
                AddComponent(new RotationComponent()
                {
                    RotationSpeed = authoring.rotationSpeed
                });
            }
        }
    }
}