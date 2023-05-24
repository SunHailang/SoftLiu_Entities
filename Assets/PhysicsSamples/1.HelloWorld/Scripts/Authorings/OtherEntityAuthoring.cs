using Unity.Entities;
using UnityEngine;

namespace PhysicsSamples.HelloWorld
{
    public class OtherEntityAuthoring : MonoBehaviour
    {
        private class Baker : Baker<OtherEntityAuthoring>
        {
            public override void Bake(OtherEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddSharedComponent(entity, new EntitySharedComponent
                {
                    EntityType = 0
                });
            }
        }
    }
}