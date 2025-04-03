using System;
using Status;
using UnityEngine;

namespace Capsule
{
    /// <summary>
    /// カプセルの動き
    /// </summary>
    public class CapsuleMove : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;

        /// <summary>
        /// カプセル自動消滅時間
        /// </summary>
        private float automaticExtinguishingTime = 30f;

        /// <summary>
        /// カプセル移動速度
        /// </summary>
        private float capsuleMoveSpeedStage2 = 0.1f;
        private float capsuleMoveSpeedStage3 = 0.5f;

        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
        }

        private void Start()
        {
            Destroy(gameObject, automaticExtinguishingTime);
        }

        private void FixedUpdate()
        {
            var _x = transform.position.x;
            var _y = transform.position.y;
            var _z = transform.position.z;

            switch (_gameState)
            {
                case GameState.Stage1:
                    return;
                case GameState.Stage2:
                    _x += capsuleMoveSpeedStage2;
                    transform.position = new Vector3(_x, _y, _z);
                    return;
                case GameState.Stage2Boss:
                    _x += capsuleMoveSpeedStage2;
                    transform.position = new Vector3(_x, _y, _z);
                    return;
                case GameState.Stage3:
                    _x -= capsuleMoveSpeedStage3;
                    transform.position = new Vector3(_x, _y, _z);
                    return;
            }
        }
    }
}
