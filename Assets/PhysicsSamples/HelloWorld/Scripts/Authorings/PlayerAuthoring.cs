using Unity.Entities;
using UnityEngine;

namespace PhysicsSamples.HelloWorld
{
    public class PlayerAuthoring : MonoBehaviour
    {

        private class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTagComponent>(entity);
                
                AddSharedComponent(entity, new EntitySharedComponent
                {
                    EntityType = 1
                });
            }
        }
    }
}
