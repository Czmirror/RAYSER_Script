using Status;
using UniRx;
using UnityEngine;

namespace Obstacle
{
    public class Stage1WallVisible : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Gamestart || x == GameState.Stage2Interval)
                .Subscribe(_ => WallHidden())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage1)
                .Subscribe(_ => WallVisible())
                .AddTo(this);
        }

        private void WallVisible()
        {
            gameObject.SetActive(true);
        }

        private void WallHidden()
        {
            gameObject.SetActive(false);
        }
    }
}
