using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// トップビュー用移動クラス
    /// </summary>
    public class TopViewMove : IMovable
    {
        /// <summary>
        /// 自機移動用クラス
        /// </summary>
        PlayerMoveCore _currentPlayerMoveCore;

        /// <summary>
        /// 操作ボタンの値
        /// </summary>
        private Vector2 _moveDirection;

        /// <summary>
        /// 移動制限区域
        /// </summary>
        [SerializeField] private float[] restrictedMovementArea = { -50f, 50f, -50f, 50f };

        public void Initialize(PlayerMoveCore playerMoveCore)
        {
            _currentPlayerMoveCore = playerMoveCore;
        }

        public void Move()
        {
            var horizon = _moveDirection.x * _currentPlayerMoveCore.CurrentSpeed();
            var vertical = _moveDirection.y * _currentPlayerMoveCore.CurrentSpeed();

            _currentPlayerMoveCore.transform.position +=
                new Vector3(vertical * -1 * Time.deltaTime, 0, horizon * Time.deltaTime);

            var direction = new Vector3(horizon, 0, vertical);
            if (direction != Vector3.zero)
            {
                _currentPlayerMoveCore.transform.localRotation = Quaternion.LookRotation(direction);
            }

            MovementRestrictions();
        }

        public void SetDirection(Vector2 moveValue)
        {
            _moveDirection = moveValue;
        }

        public void MovementRestrictions()
        {
            var playerPosionX = Mathf.Clamp(_currentPlayerMoveCore.transform.position.x, restrictedMovementArea[0],
                restrictedMovementArea[1]);
            var playerPosionZ = Mathf.Clamp(_currentPlayerMoveCore.transform.position.z, restrictedMovementArea[2],
                restrictedMovementArea[3]);
            _currentPlayerMoveCore.transform.position =
                new Vector3(playerPosionX, _currentPlayerMoveCore.transform.position.y, playerPosionZ);
        }
    }
}
