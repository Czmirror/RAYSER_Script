using System;
using UnityEngine;

namespace Turret
{
    /// <summary>
    /// マズルフラッシュ、ヒットフラッシュ用クラス（未使用）
    /// </summary>
    public class MuzzleAndHitFlash : MonoBehaviour
    {
        /// <summary>
        /// ゲームオブジェクトを向ける方向
        /// </summary>
        private Transform lookAtTransform;

        /// <summary>
        /// ゲームオブジェクトの発生位置
        /// </summary>
        private Transform instantiateTransform;
        public void Initialize(Transform instantiate,Transform target)
        {
            instantiateTransform = instantiate;
            lookAtTransform = target;
        }

        private void Update()
        {
            transform.position = instantiateTransform.position;
            transform.LookAt(lookAtTransform);
        }
    }
}
