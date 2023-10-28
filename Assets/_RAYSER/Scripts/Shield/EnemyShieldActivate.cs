using System;
using Status;
using UniRx;
using UnityEngine;

namespace Shield
{
    /// <summary>
    /// 対象シーンでシールドを有効にする（攻撃判定を有効化）
    /// </summary>
    public class EnemyShieldActivate : MonoBehaviour
    {
        /// <summary>
        /// コライダーを有効にするゲームシーンステート
        /// </summary>
        [SerializeField] private GameState _targetGameState;

        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        private BoxCollider _boxCollider;

        private void Start()
        {
            _boxCollider = gameObject.GetComponent<BoxCollider>();
            _boxCollider.enabled = false;

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == _targetGameState
                )
                .Subscribe(_ => BoxColliderActivate())
                .AddTo(this);
        }

        private void BoxColliderActivate()
        {
            _boxCollider.enabled = true;
        }
    }
}
