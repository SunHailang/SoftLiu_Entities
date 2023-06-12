using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    [BurstCompile]
    public partial class CameraControllerSystem : SystemBase
    {
        private Entity _entity;
        private Unity.Mathematics.Random _random;

        [BurstCompile]
        protected override void OnCreate()
        {
            _random = Unity.Mathematics.Random.CreateFromIndex(123);
        }
        
        protected override void OnUpdate()
        {
            if (_entity == Entity.Null || Input.GetKeyDown(KeyCode.Space))
            {
                var tankBuilder = SystemAPI.QueryBuilder().WithAll<TankMoveComponent>().Build();
                var tanks = tankBuilder.ToEntityArray(Allocator.Temp);
                if(tanks.Length <= 0) return;
                _entity = tanks[_random.NextInt(tanks.Length)];
            }
        
            var cameraSingleton = CameraSingleton.Instance;
            if(cameraSingleton == null) return;
            var entityTransform = SystemAPI.GetComponent<LocalTransform>(_entity);
            cameraSingleton.transform.position = entityTransform.Position;
            cameraSingleton.transform.position -= 10 * (Vector3) entityTransform.Forward();
            cameraSingleton.transform.position += new Vector3(0, 5.0f, 0);
        
            cameraSingleton.transform.LookAt(entityTransform.Position);
        }
    }
}