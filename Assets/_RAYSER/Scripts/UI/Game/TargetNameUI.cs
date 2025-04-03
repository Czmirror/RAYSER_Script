using System;
using _RAYSER.Scripts.Event.Signal;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class TargetNameUI : MonoBehaviour
    {
        [Header("UI References")] [SerializeField]
        private TextMeshProUGUI targetNameTextMeshPro;

        [SerializeField] private CanvasGroup targetNameCanvasGroup;

        private void Start()
        {
            targetNameCanvasGroup.alpha = 0;
            targetNameTextMeshPro.text = string.Empty;

            MessageBroker.Default.Receive<TutorialStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => { HideTargetName(); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialLaser>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => { HideTargetName(); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialItem>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => { HideTargetName(); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialSubWeapon>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => { HideTargetName(); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialBomb>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => { HideTargetName(); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialEnd>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => { HideTargetName(); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialStart>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => { ViewTargetName("左スティックで移動しよう"); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialLaser>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => { ViewTargetName("レーザーで敵機を撃墜しよう"); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialItem>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => { ViewTargetName("アイテムを取得しよう"); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialSubWeapon>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => { ViewTargetName("サブウェポンを発射しよう"); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialBomb>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => { ViewTargetName("ボムを使用しよう"); })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialEnd>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => { HideTargetName(); })
                .AddTo(this);
        }


        private void ViewTargetName(string targetName)
        {
            var sequence = DOTween.Sequence();

            sequence
                .Append(targetNameCanvasGroup.DOFade(1f, 0.5f))
                .Append(targetNameTextMeshPro.DOText(String.Empty, 0))
                .Append(targetNameTextMeshPro.DOText(targetName, 0.5f))
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);
            sequence.Restart();
        }

        private void HideTargetName()
        {
            var sequence = DOTween.Sequence();

            sequence
                .Append(targetNameTextMeshPro.DOText(String.Empty, 0))
                .Append(targetNameCanvasGroup.DOFade(0f, 0.5f))
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);
            sequence.Restart();
        }
    }
}
