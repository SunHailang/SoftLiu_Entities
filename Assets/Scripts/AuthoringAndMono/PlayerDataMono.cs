using Unity.Entities;
using UnityEngine;

namespace SoftLiu.Player
{
    public class PlayerDataMono : MonoBehaviour
    {
        public float MoveSpeed;
    }

    // public class PlayerDataBaker : Baker<PlayerDataMono>
    // {
    //     public override void Bake(PlayerDataMono authoring)
    //     {
    //         AddComponent<PlayerMoveComponent>(new PlayerMoveComponent()
    //         {
    //             MoveSpeed = authoring.MoveSpeed
    //         });
    //     }
    // }
}