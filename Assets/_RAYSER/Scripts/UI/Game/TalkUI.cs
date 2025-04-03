using System;
using System.Collections.Generic;
using _RAYSER.Scripts.Event.Signal;
using DG.Tweening;
using Event.Signal;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Game
{
    public class TalkUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform talkUIRectTransform;
        [SerializeField] private CanvasGroup talkUICanvasGroup;
        [SerializeField] private CanvasGroup talkWindowUICanvasGroup;
        [SerializeField] private CanvasGroup roydFaceWindowCanvasGroup;
        [SerializeField] private CanvasGroup sophieFaceWindowCanvasGroup;
        [SerializeField] private TextMeshProUGUI talkTextMeshPro;

        [Header("ScriptableObjects for each scenario")]
        [SerializeField] private TalkConversation stage1BossConversation;
        [SerializeField] private TalkConversation stage2IntervalConversation;
        [SerializeField] private TalkConversation stage2BossConversation;
        [SerializeField] private TalkConversation stage3IntervalConversation;
        [SerializeField] private TalkConversation gameClearConversation;
        [SerializeField] private TalkConversation tutorialStartConversation;
        [SerializeField] private TalkConversation tutorialLaserConversation;
        [SerializeField] private TalkConversation tutorialItemConversation;
        [SerializeField] private TalkConversation tutorialSubWeaponConversation;
        [SerializeField] private TalkConversation tutorialBombConversation;
        [SerializeField] private TalkConversation tutorialEndConversation;

        private Vector3 talkUIPosition;
        private Vector3 _outUIPosition;

        private String _messege;
        private float _messageSpeed = 0.05f;

        /// <summary>
        /// キャラクターごとの FaceWindow を一括で管理するための辞書
        /// </summary>
        private Dictionary<TalkCharacter, CanvasGroup> faceWindowMap = new Dictionary<TalkCharacter, CanvasGroup>();

        /// <summary>
        /// 今アクティブなキャラクターの CanvasGroup
        /// </summary>
        private CanvasGroup currentActiveFace;

        private void Start()
        {
            talkUIPosition = talkUIRectTransform.localPosition;
            InitializeUI();

            // FaceWindow をキャラクターごとに辞書にセットしておく
            faceWindowMap[TalkCharacter.Royd] = roydFaceWindowCanvasGroup;
            faceWindowMap[TalkCharacter.Sophie] = sophieFaceWindowCanvasGroup;

            MessageBroker.Default.Receive<TutorialStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ =>
                {
                    StartConversation(tutorialStartConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new TutorialStart(TalkEnum.TalkEnd));
                            MessageBroker.Default.Publish(new TutorialMove());
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialLaser>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ =>
                {
                    StartConversation(tutorialLaserConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new TutorialLaser(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialItem>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ =>
                {
                    StartConversation(tutorialItemConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new TutorialItem(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialSubWeapon>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ =>
                {
                    StartConversation(tutorialSubWeaponConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new TutorialSubWeapon(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialBomb>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ =>
                {
                    StartConversation(tutorialBombConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new TutorialBomb(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<TutorialEnd>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ =>
                {
                    StartConversation(tutorialEndConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new TutorialEnd(TalkEnum.TalkEnd));
                            SceneManager.LoadScene("Game");
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage1BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    StartConversation(stage1BossConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new Stage1BossEncounter(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage2IntervalStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    StartConversation(stage2IntervalConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new Stage2IntervalStart(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage2BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    StartConversation(stage2BossConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new Stage2BossEncounter(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage3IntervalStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    StartConversation(stage3IntervalConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new Stage3IntervalStart(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);

            MessageBroker.Default.Receive<GameClear>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    StartConversation(gameClearConversation,
                        onComplete: () =>
                        {
                            MessageBroker.Default.Publish(new GameClear(TalkEnum.TalkEnd));
                        });
                })
                .AddTo(this);
        }

        private void InitializeUI()
        {
            _outUIPosition = new Vector3(2000, 0, 0);
            talkUIRectTransform.localPosition = _outUIPosition;
            talkUICanvasGroup.alpha = 0;
            talkWindowUICanvasGroup.alpha = 0;
            roydFaceWindowCanvasGroup.alpha = 0;
            sophieFaceWindowCanvasGroup.alpha = 0;
            talkTextMeshPro.text = String.Empty;
        }

        /// <summary>
        /// 会話開始処理
        /// </summary>
        /// <returns>DOTweenシーケンス</returns>
        private Sequence TalkStart()
        {
            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            talkUIRectTransform.localPosition = talkUIPosition;
            sequence
                .Append(talkUICanvasGroup.DOFade(1f, 0.5f))
                .AppendInterval(0.5f);

            return sequence;
        }

        /// <summary>
        /// 会話終了処理
        /// </summary>
        /// <param name="sequence">DOTweenシーケンス</param>
        private void Finalization(Sequence sequence)
        {
            sequence
                .Append(talkUICanvasGroup.DOFade(0f, 1f))
                .Append(talkWindowUICanvasGroup.DOFade(0f, 1f))
                .Join(roydFaceWindowCanvasGroup.DOFade(0f, 1f))
                .Join(sophieFaceWindowCanvasGroup.DOFade(0f, 1f))
                .AppendCallback(() => { InitializeUI(); })
                .AppendInterval(0.5f)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);
            sequence.Restart();
        }

        /// <summary>
        /// 共通の会話再生メソッド
        /// </summary>
        /// <param name="conversation">ScriptableObject で定義した会話データ</param>
        /// <param name="onComplete">会話終了時のコールバック</param>
        private void StartConversation(TalkConversation conversation, Action onComplete = null)
        {
            var sequence = TalkStart();

            // 会話ウィンドウをフェードイン
            sequence.Append(talkWindowUICanvasGroup.DOFade(1f, 0.5f));

            // ScriptableObject に定義された会話分だけループしてシーケンスを積み上げる
            foreach (var sentence in conversation.Sentences)
            {
                // キャラクターの切り替え
                var fadeList = SwitchFaceWindow(sentence.Character);
                foreach (var fade in fadeList)
                {
                    sequence.Append(fade);
                }

                // テキストをクリアしてから新しい文章を表示
                sequence.Append(talkTextMeshPro.DOText(string.Empty, 0f));
                sequence.Append(talkTextMeshPro.DOText(sentence.Message, sentence.Message.Length * _messageSpeed)
                    .SetEase(Ease.Linear));

                // 文章ごとに少し間を取る
                sequence.AppendInterval(0.5f);
            }

            // 全文表示し終わったら onComplete を実行
            sequence.OnComplete(() =>
            {
                onComplete?.Invoke();
            });

            // その後のフェードアウトなどの後処理
            Finalization(sequence);
        }

        /// <summary>
        /// 顔ウィンドウをキャラクターごとに切り替える
        /// </summary>
        /// <param name="character">話すキャラクター</param>
        private List<Tween> SwitchFaceWindow(TalkCharacter character)
        {
            // 新しくアクティブにしたいウィンドウ
            var newFaceWindow = faceWindowMap[character];

            var fadeList = new List<Tween>();

            // 既に何かアクティブなキャラウィンドウがあり、かつ別キャラならフェードアウト
            if (currentActiveFace != null && currentActiveFace != newFaceWindow)
            {
                currentActiveFace.DOFade(0f, 0.3f);
                fadeList.Add(currentActiveFace.DOFade(0f, 0.3f));
            }

            // 新しいキャラのウィンドウが非表示ならフェードイン
            if (newFaceWindow.alpha < 1f)
            {
                newFaceWindow.DOFade(1f, 0.5f);
                fadeList.Add(newFaceWindow.DOFade(1f, 0.5f));
            }

            currentActiveFace = newFaceWindow;
            return fadeList;
        }
    }
}
