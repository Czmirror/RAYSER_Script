using System;
using System.Collections.Generic;
using Target;
using Turret;
using UnityEngine;

namespace _RAYSER.Scripts.Turret.FirePointProvider
{
    /// <summary>
    /// 自機をターゲットとするプロバイダー
    /// </summary>
    [Serializable]
    public class TargetedFirePointProvider: IFirePointProvider
    {
        [SerializeField] private Transform targetTransform;
        private Transform ownerTransform;
        private List<FirePoint> firePoints;

        public void Initialize(Transform ownerTransform, List<FirePoint> firePoints)
        {
            this.ownerTransform = ownerTransform;
            this.firePoints = firePoints;
        }
        public List<Vector3> GetPositions()
        {
            if (firePoints == null || ownerTransform == null)
            {
                Debug.LogError("TargetedFirePointProvider is not properly initialized.");
                return new List<Vector3>();
            }

            return firePoints.ConvertAll(fp => ownerTransform.TransformPoint(fp.Position));
        }

        public List<Quaternion> GetRotations()
        {
            if (firePoints == null || ownerTransform == null)
            {
                Debug.LogError("TargetedFirePointProvider is not properly initialized.");
                return new List<Quaternion>();
            }

            // ターゲットが存在する場合、その方向に回転
            return firePoints.ConvertAll(fp =>
            {
                var position = ownerTransform.TransformPoint(fp.Position);
                if (targetTransform != null)
                {
                    var direction = (targetTransform.position - position).normalized;
                    return Quaternion.LookRotation(direction);
                }

                // ターゲットがない場合は現在の回転を維持
                return ownerTransform.rotation;
            });
        }
    }
}
