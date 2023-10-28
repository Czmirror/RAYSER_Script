using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// フロントビュー用移動クラス
    /// </summary>
    public class FrontViewMove : IMovable
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
        [SerializeField] private float[] restrictedMovementArea = { 0f, 60f, -150f, -50f };

        /// <summary>
        /// 現在のスピードレベル
        /// </summary>
        private int speedLeve = 1;

        private void Start()
        {

        }

        public void Initialize(PlayerMoveCore playerMoveCore)
        {
            _currentPlayerMoveCore = playerMoveCore;
        }

        public void Move()
        {
            var horizon = _moveDirection.x * _currentPlayerMoveCore.CurrentSpeed();
            var vertical = _moveDirection.y * _currentPlayerMoveCore.CurrentSpeed();

            _currentPlayerMoveCore.transform.position +=
                new Vector3(0, vertical * Time.deltaTime, horizon * -1 * Time.deltaTime);
            MovementRestrictions();
        }

        public void SetDirection(Vector2 moveValue)
        {
            _moveDirection = moveValue;
        }

        public void MovementRestrictions()
        {
            var playerPosionY = Mathf.Clamp(_currentPlayerMoveCore.transform.position.y, restrictedMovementArea[0],
                restrictedMovementArea[1]);
            var playerPosionZ = Mathf.Clamp(_currentPlayerMoveCore.transform.position.z, restrictedMovementArea[2],
                restrictedMovementArea[3]);
            _currentPlayerMoveCore.transform.position =
                new Vector3(_currentPlayerMoveCore.transform.position.x, playerPosionY, playerPosionZ);
        }
    }
}
