using _RAYSER.Scripts.Event.Signal;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// サイドビュー用移動クラス
    /// </summary>
    public class SideViewMove : IMovable
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
        /// 回転検知用位置情報
        /// </summary>
        private Vector2 _previousDirection = Vector2.zero;

        /// <summary>
        /// 移動制限区域
        /// </summary>
        // [SerializeField] private float[] restrictedMovementArea = { -817f, -782f, -50f, 50f };
        [SerializeField] private float[] restrictedMovementArea = { -822f, -777f, -50f, 50f };

        public void Initialize(PlayerMoveCore playerMoveCore)
        {
            _currentPlayerMoveCore = playerMoveCore;
        }

        public void Move()
        {
            var horizon = _moveDirection.x * _currentPlayerMoveCore.CurrentSpeed();
            var vertical = _moveDirection.y * _currentPlayerMoveCore.CurrentSpeed();

            _currentPlayerMoveCore.transform.position +=
                new Vector3(horizon * -1 * Time.deltaTime, vertical * Time.deltaTime, 0);

            // 進行方向の反対に進み始めたらローリング発生
            if ((_previousDirection.x > 0 && _moveDirection.x < 0) ||
                (_previousDirection.x < 0 && _moveDirection.x > 0) ||
                (_previousDirection.y > 0 && _moveDirection.y < 0) ||
                (_previousDirection.y < 0 && _moveDirection.y > 0))
            {
                Vector3 rotationAxis = (_moveDirection.x > 0) ? Vector3.right : Vector3.left;  // X軸回転
                float rotationAngle = (_moveDirection.x > 0) ? 360f : -360f; // 右→左なら時計回り、左→右なら反時計回り

                MessageBroker.Default.Publish(new RollingSignal(rotationAxis, rotationAngle));
            }

            _previousDirection = _moveDirection;

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
            var playerPosionY = Mathf.Clamp(_currentPlayerMoveCore.transform.position.y, restrictedMovementArea[2],
                restrictedMovementArea[3]);
            _currentPlayerMoveCore.transform.position =
                new Vector3(playerPosionX, playerPosionY, _currentPlayerMoveCore.transform.position.z);
        }
    }
}
