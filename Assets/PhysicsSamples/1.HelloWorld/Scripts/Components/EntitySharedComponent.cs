using Unity.Entities;

namespace PhysicsSamples.HelloWorld
{
    public struct EntitySharedComponent : ISharedComponentData
    {
        /// <summary>
        /// 玩家：1， 敌人：2， 建筑物：0
        /// </summary>
        public uint EntityType;
    }
}