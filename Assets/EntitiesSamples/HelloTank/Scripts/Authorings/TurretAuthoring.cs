
using Unity.Entities;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    public class TurretAuthoring : MonoBehaviour
    {
        public float rotationSpeed;

        class Baker : Baker<TurretAuthoring>
        {
            public override void Bake(TurretAuthoring authoring)
            {
                AddComponent(new TurretComponent
                {
                    RotationSpeed = authoring.rotationSpeed
                });
            }
        }
    }
}