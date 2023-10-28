using System;
using DG.Tweening;
using Event.Signal;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class TalkUI : MonoBehaviour
    {
        [SerializeField] private RectTransform talkUIRectTransform;
        [SerializeField] private CanvasGroup talkUICanvasGroup;
        [SerializeField] private CanvasGroup talkWindowUICanvasGroup;
        [SerializeField] private CanvasGroup roydFaceWindowCanvasGroup;
        [SerializeField] private CanvasGroup sophieFaceWindowCanvasGroup;

        private Vector3 talkUIPosition;
        private Vector3 _outUIPosition;
        [SerializeField] private TextMeshProUGUI talkTextMeshPro;
        private String _messege;
        private float _message_speed = 0.05f;

        private void Start()
        {
            talkUIPosition = talkUIRectTransform.localPosition;
            InitializeUI();

            MessageBroker.Default.Receive<Stage1BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x => Stage1BossTalk())
                .AddTo(this);

            MessageBroker.Default.Receive<Stage2IntervalStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x => Stage2IntervalTalk())
                .AddTo(this);

            MessageBroker.Default.Receive<Stage2BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x => Stage2BossTalk())
                .AddTo(this);

            MessageBroker.Default.Receive<Stage3IntervalStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x => Stage3IntervalTalk())
                .AddTo(this);

            MessageBroker.Default.Receive<GameClear>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x => GameClearTalk())
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

        private void Stage1BossTalk()
        {
            var sequence = TalkStart();

            _messege = "ロイド、敵の迎撃部隊よ！気をつけて！";
            sequence
                .Append(talkWindowUICanvasGroup.DOFade(1f, 0.5f))
                .Join(sophieFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "了解、これより敵迎撃部隊と交戦する。";
            sequence
                .Append(sophieFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f)
                .OnComplete(() => MessageBroker.Default.Publish(new Stage1BossEncounter(TalkEnum.TalkEnd)));

            Finalization(sequence);
        }

        private void Stage2IntervalTalk()
        {
            var sequence = TalkStart();

            _messege = "ソフィー、この辺りの敵勢力を撃破した。";
            sequence
                .Append(talkWindowUICanvasGroup.DOFade(1f, 0.5f))
                .Join(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "了解ロイド、それでは武装の少ない敵戦艦の側部から回り込んで、戦艦の後方部へ向かって。";
            sequence
                .Append(roydFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(sophieFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "了解、これより敵戦艦側部から侵攻を開始する。";
            sequence
                .Append(sophieFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f)
                .OnComplete(() => MessageBroker.Default.Publish(new Stage2IntervalStart(TalkEnum.TalkEnd)));

            Finalization(sequence);
        }

        private void Stage2BossTalk()
        {
            var sequence = TalkStart();

            _messege = "ロイド、敵戦艦からの迎撃部隊が発進されたわ！気をつけて！";
            sequence
                .Append(talkWindowUICanvasGroup.DOFade(1f, 0.5f))
                .Join(sophieFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "了解、これより敵迎撃部隊と交戦する。";
            sequence
                .Append(sophieFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f)
                .OnComplete(() => MessageBroker.Default.Publish(new Stage2BossEncounter(TalkEnum.TalkEnd)));

            Finalization(sequence);
        }

        private void Stage3IntervalTalk()
        {
            var sequence = TalkStart();

            _messege = "ソフィー、敵戦艦の後方部へ到着した。";
            sequence
                .Append(talkWindowUICanvasGroup.DOFade(1f, 0.5f))
                .Join(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "了解ロイド、敵戦艦の動力部をなんとか破壊して、そうすれば主力部隊が追いつけるようになるわ。";
            sequence
                .Append(roydFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(sophieFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "了解、これより敵戦艦動力部の破壊を試みる。";
            sequence
                .Append(sophieFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f)
                .OnComplete(() => MessageBroker.Default.Publish(new Stage3IntervalStart(TalkEnum.TalkEnd)));

            Finalization(sequence);
        }

        /// <summary>
        /// ゲームクリア会話
        /// </summary>
        private void GameClearTalk()
        {
            var sequence = TalkStart();

            _messege = "ソフィー、敵戦艦動力部の破壊に成功した。";
            sequence
                .Append(talkWindowUICanvasGroup.DOFade(1f, 0.5f))
                .Join(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "ありがとうロイド、これであとは主力部隊が敵戦艦の破壊を実施できるわ。あなたはここから帰還して。";
            sequence
                .Append(roydFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(sophieFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f);

            _messege = "了解、これより帰還する。";
            sequence
                .Append(sophieFaceWindowCanvasGroup.DOFade(0f, 0.5f))
                .Append(roydFaceWindowCanvasGroup.DOFade(1f, 0.5f))
                .Append(talkTextMeshPro.DOText(String.Empty, 0))
                .Append(talkTextMeshPro.DOText(_messege, _messege.Length * _message_speed).SetEase(Ease.Linear))
                .AppendInterval(0.5f)
                .OnComplete(() => MessageBroker.Default.Publish(new GameClear(TalkEnum.TalkEnd)));

            Finalization(sequence);
        }
    }
}
