using _RAYSER.Scripts.Event.Signal;
using Cysharp.Threading.Tasks;
using UniRx;
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
        private PlayerMoveCore _currentPlayerMoveCore;

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
        [SerializeField] private float[] _restrictedMovementArea = { 0f, 60f, -150f, -50f };

        /// <summary>
        /// 現在のスピードレベル
        /// </summary>
        private int _speedLeve = 1;

        /// <summary>
        /// 回転速度
        /// </summary>
        private readonly float _rollingDuration = 0.5f;

        private FrontViewTiltHandler _tiltHandler;

        private void Start()
        {
        }

        public void Initialize(PlayerMoveCore playerMoveCore)
        {
            _currentPlayerMoveCore = playerMoveCore;
        }

        public void SetFrontViewTiltHandler(FrontViewTiltHandler tiltHandler)
        {
            _tiltHandler = tiltHandler;
        }

        public void Move()
        {
            var horizon = _moveDirection.x * _currentPlayerMoveCore.CurrentSpeed();
            var vertical = _moveDirection.y * _currentPlayerMoveCore.CurrentSpeed();

            _currentPlayerMoveCore.transform.position +=
                new Vector3(0, vertical * Time.deltaTime, horizon * -1 * Time.deltaTime);

            // **自機を傾ける**
            _tiltHandler.ApplyTilt(_moveDirection.x);

            // 進行方向の反対に進み始めたらローリング発生
            if ((_previousDirection.x > 0 && _moveDirection.x < 0) ||
                (_previousDirection.x < 0 && _moveDirection.x > 0))
            {
                Vector3 rotationAxis = (_moveDirection.x > 0) ? Vector3.right : Vector3.left; // X軸回転
                float rotationAngle = (_moveDirection.x > 0) ? 360f : -360f; // 右→左なら時計回り、左→右なら反時計回り

                MessageBroker.Default.Publish(new RollingSignal(rotationAxis, rotationAngle, _rollingDuration));
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
            var playerPosionY = Mathf.Clamp(_currentPlayerMoveCore.transform.position.y, _restrictedMovementArea[0],
                _restrictedMovementArea[1]);
            var playerPosionZ = Mathf.Clamp(_currentPlayerMoveCore.transform.position.z, _restrictedMovementArea[2],
                _restrictedMovementArea[3]);
            _currentPlayerMoveCore.transform.position =
                new Vector3(_currentPlayerMoveCore.transform.position.x, playerPosionY, playerPosionZ);
        }
    }
}
