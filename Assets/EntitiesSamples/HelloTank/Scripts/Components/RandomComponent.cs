using Unity.Entities;
using Unity.Mathematics;

namespace EntitiesSamples.HelloTank
{
    public partial struct RandomComponent : IComponentData
    {
        public Random Value;
    }
}