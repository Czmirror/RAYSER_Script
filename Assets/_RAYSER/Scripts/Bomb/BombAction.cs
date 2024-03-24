using UnityEngine;

namespace _RAYSER.Scripts.Bomb
{
    /// <summary>
    /// ボムの発射処理（Element）
    /// </summary>
    public class BombAction
    {
        public Vector3 Position { get; private set; }

        public BombAction(Vector3 position)
        {
            Position = position;
        }

        public void Accept(IBombVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
