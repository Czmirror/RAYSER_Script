using System.Collections.Generic;
using Turret;
using UnityEngine;

namespace _RAYSER.Scripts.Turret.FirePointProvider
{
    public interface IFirePointProvider
    {
        void Initialize(Transform ownerTransform, List<FirePoint> firePoints);
        List<Vector3> GetPositions();
        List<Quaternion> GetRotations();
    }
}
