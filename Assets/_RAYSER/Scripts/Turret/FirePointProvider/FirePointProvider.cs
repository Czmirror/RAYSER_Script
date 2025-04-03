using System.Collections.Generic;
using Turret;
using UnityEngine;

namespace _RAYSER.Scripts.Turret.FirePointProvider
{
    /// <summary>
    /// 弾座標提供
    /// </summary>
    public class FirePointProvider : IFirePointProvider
    {
        private Transform ownerTransform;
        private List<FirePoint> firePoints;

        public void Initialize(Transform ownerTransform, List<FirePoint> firePoints)
        {
            this.ownerTransform = ownerTransform;
            this.firePoints = firePoints;
        }

        public List<Vector3> GetPositions()
        {
            return firePoints.ConvertAll(fp => ownerTransform.TransformPoint(fp.Position));
        }

        public List<Quaternion> GetRotations()
        {
            return firePoints.ConvertAll(fp => ownerTransform.rotation * Quaternion.Euler(fp.Rotation));
        }
    }
}
