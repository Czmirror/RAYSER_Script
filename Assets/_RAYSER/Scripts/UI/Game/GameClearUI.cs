using System;
using DG.Tweening;
using Event;
using Event.Signal;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Game
{
    public class GameClearUI : MonoBehaviour
    {
        [SerializeField] private RectTransform gameClearUIRectTransform;
        [SerializeField] private CanvasGroup gameClearUICanvasGroup;
        [SerializeField] private CanvasGroup missionCompleteUICanvasGroup;
        [SerializeField] private CanvasGroup buttonsUICanvasGroup;
        [SerializeField] private CanvasGroup scoreUICanvasGroup;
        [SerializeField] private CanvasGroup roydImageUICanvasGroup;
        [SerializeField] private GameObject _roydGameObject;
        [SerializeField] private GameObject canvasGame;

        /// <summary>
        /// 最初にフォーカスになるUIボタン
        /// </summary>
        [SerializeField] private GameObject uiFirstFocusButton;

        private Vector3 _inUIPosition;
        private Vector3 _outUIPosition;
        private void Start()
        {
            InitializeUI();
            MessageBroker.Default.Receive<GameClear>().Where(x=>x._talk == TalkEnum.TalkEnd).Subscribe(x => GameClearUIVisible()).AddTo(this);

            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);
            gameObject.SetActive(false);
        }

        private void InitializeUI()
        {
            _roydGameObject.SetActive(false);
            gameClearUICanvasGroup.alpha = 0;
            missionCompleteUICanvasGroup.alpha = 0;
            buttonsUICanvasGroup.alpha = 0;
            scoreUICanvasGroup.alpha = 0;
            roydImageUICanvasGroup.alpha = 0;
            _inUIPosition = new Vector3(0, 0, 0);
            _outUIPosition = new Vector3(2000, 0, 0);
            gameClearUIRectTransform.localPosition = _outUIPosition;

        }

        private void GameClearUIVisible()
        {
            gameObject.SetActive(true);
            canvasGame.SetActive(false);
            // _roydGameObject.SetActive(true);

            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            gameClearUIRectTransform.localPosition = _inUIPosition;
            sequence
                .Append(gameClearUICanvasGroup.DOFade(1f, 2f))
                .AppendInterval(1f)
                .OnStart(() =>
                {
                    //初期選択ボタンの再指定
                    EventSystem.current.SetSelectedGameObject(uiFirstFocusButton);
                });;

            sequence
                .Append(missionCompleteUICanvasGroup.DOFade(1f, 1f))
                .AppendInterval(1f);

            sequence
                .Append(buttonsUICanvasGroup.DOFade(1f, 1f))
                // .Join(roydImageUICanvasGroup.DOFade(1f, 1f))
                .Join(scoreUICanvasGroup.DOFade(1f, 1f))
                .AppendInterval(1f)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            sequence.Restart();
        }
    }
}
