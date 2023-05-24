using PhysicsSamples.HelloWorld;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace PhysicsSamples.Common
{
    public partial class CameraCtrlSystem : SystemBase
    {
        private bool isFirst = true;

        private bool isLock = false;

        protected override void OnCreate()
        {
            base.OnCreate();
            isFirst = true;
            // 读取配置
            isLock = false;
        }


        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                isLock = !isLock;
            }
            
            var input = Input.GetKey(KeyCode.Space);
            if ((input || isFirst || isLock) && Camera.main != null)
            {
                foreach (var transform in SystemAPI.Query<LocalTransform>().WithAll<LockCameraTagComponent>())
                {
                    Camera.main.transform.position = transform.Position + new float3(0f, 5.2f, -1.6f);
                    isFirst = false;
                }
            }
        }
    }
}