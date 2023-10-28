using System;
using UnityEngine;

namespace Explosion
{
    public class Explode : MonoBehaviour
    {
        /// <summary>
        /// 消滅時間
        /// </summary>
        [SerializeField] private float extinguishedTime = 1f;
        private void Start()
        {
            Destroy(gameObject, extinguishedTime);
        }
    }
}
