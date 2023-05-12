using Unity.Entities;
using Unity.Transforms;

namespace EntitiesSamples.HelloTank
{
    public readonly partial struct BulletAspect : IAspect
    {
        public readonly Entity Entity;
        private readonly TransformAspect _transformAspect;

        private readonly RefRO<BulletMoveComponent> _bulletMoveComponent;
    }
}