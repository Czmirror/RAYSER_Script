using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// サブウェポンの発射処理（Element）
    /// </summary>
    public class FireAction
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public FireAction(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void Accept(ISubWeaponVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
