using System;
using Status;
using UniRx;
using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// 自機バックファイアエフェクト表示制御
    /// </summary>
    public class PlayerMoveBackFIreEffect: MonoBehaviour
    {
        [SerializeField] private GameStatus _gameStatus;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Gamestart ||
                    x == GameState.Stage2Interval ||
                    x == GameState.Stage3Interval
                )
                .Subscribe(_ => EffectHidden())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage1 ||
                    x == GameState.Stage1Boss
                )
                .Subscribe(_ => EffectShow())
                .AddTo(this);
        }

        private void EffectHidden()
        {
            gameObject.SetActive(false);
        }

        private void EffectShow()
        {
            gameObject.SetActive(true);
        }
    }
}
