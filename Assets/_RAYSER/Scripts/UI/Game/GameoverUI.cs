using System;
using DG.Tweening;
using Event;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.Game
{
    public class GameoverUI : MonoBehaviour
    {
        [SerializeField] private RectTransform gameoverUIRectTransform;
        [SerializeField] private CanvasGroup gameoverUICanvasGroup;
        [SerializeField] private CanvasGroup MissionFailedUICanvasGroup;
        [SerializeField] private CanvasGroup ButtonsUICanvasGroup;
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
            MessageBroker.Default.Receive<Gameover>().Subscribe(x => Gameover()).AddTo(this);

            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);
            gameObject.SetActive(false);
        }

        private void InitializeUI()
        {
            gameoverUICanvasGroup.alpha = 0;
            _inUIPosition = new Vector3(0, 0, 0);
            _outUIPosition = new Vector3(2000, 0, 0);
            gameoverUIRectTransform.localPosition = _outUIPosition;
        }

        private void Gameover()
        {
            gameObject.SetActive(true);
            canvasGame.SetActive(false);

            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            gameoverUIRectTransform.localPosition = _inUIPosition;
            sequence
                .Append(gameoverUICanvasGroup.DOFade(1f, 0.5f))
                .AppendInterval(0.5f)
                .OnStart(() =>
                {
                    //初期選択ボタンの再指定
                    EventSystem.current.SetSelectedGameObject(uiFirstFocusButton);
                });

            sequence
                .Append(MissionFailedUICanvasGroup.DOFade(1f, 0.5f))
                .AppendInterval(0.5f);

            sequence
                .Append(ButtonsUICanvasGroup.DOFade(1f, 0.5f))
                .AppendInterval(0.5f)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            sequence.Restart();
        }
    }
}
