using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    public class ConfigAuthoring : MonoBehaviour
    {
        public int tankCount;
        public GameObject tankPrefab;

        class Baker : Baker<ConfigAuthoring>
        {
            public override void Bake(ConfigAuthoring authoring)
            {
                AddComponent(new ConfigComponent
                {
                    TankCount = authoring.tankCount,
                    TankEntity = GetEntity(authoring.tankPrefab)
                });
                AddComponent(new RandomComponent
                { 
                    Value = Unity.Mathematics.Random.CreateFromIndex(51)
                });
            }
        }
    }
}
