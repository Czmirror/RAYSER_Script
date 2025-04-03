using System;
using UnityEngine;

namespace Turret
{
    /// <summary>
    /// 発射点の情報
    /// </summary>
    [Serializable]
    public class FirePoint
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }
}
