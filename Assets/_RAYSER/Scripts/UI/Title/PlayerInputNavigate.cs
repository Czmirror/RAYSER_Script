using UnityEngine;

namespace _RAYSER.Scripts.UI.Title
{
    public class PlayerInputNavigate : MonoBehaviour
    {
        private Vector2 direction;
        public void SetDirection(Vector2 moveValue)
        {
            direction = moveValue;
        }

        public Vector2 GetDirection()
        {
            return direction;
        }
    }
}
