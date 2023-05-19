using System;
using UnityEngine;

namespace EntitiesSamples.HelloTank
{
    public class CameraSingleton : MonoBehaviour
    {
        public static Camera Instance;

        private void Awake()
        {
            // if (Instance != null)
            // {
            //     Destroy(gameObject);
            //     return;
            // }
            Instance = GetComponent<Camera>();
        }
    }
}