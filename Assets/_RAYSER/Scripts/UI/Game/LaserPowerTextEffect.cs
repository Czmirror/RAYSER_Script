using System;
using DG.Tweening;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class LaserPowerTextEffect : MonoBehaviour
    {
        // [SerializeField] private CanvasGroup laserPowerTextEffectCanvasGroup;
        [SerializeField] private RectTransform laserPowerTextEffectRectTransform;

        private Vector3 _firstUIPosition = new Vector3(-110, 0, 0);
        private Vector3 _lastUIPosition = new Vector3(110, 0, 0);

        private float _uiAnimationTime = 0.5f;

        private void InitializeUI()
        {
            // laserPowerTextEffectCanvasGroup.alpha = 0;
            laserPowerTextEffectRectTransform.localPosition = _firstUIPosition;
        }

        private void Start()
        {
            InitializeUI();
            MessageBroker.Default.Receive<PlayerLaserLevelUp>().Subscribe(_ => LaserLevelUpEffect()).AddTo(this);
        }

        private void LaserLevelUpEffect()
        {
            InitializeUI();
            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            sequence
                .Append(transform.DOLocalMove(_lastUIPosition, _uiAnimationTime))
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
