using _RAYSER.Scripts.Bomb;
using _RAYSER.Scripts.Event.Signal;
using _RAYSER.Scripts.SubWeapon;
using PlayerMove;
using Shield;
using Status;
using Turret;
using UI.Game;
using UniRx;
using UnityEngine;

namespace _RAYSER.Scripts.Tutorial
{
    /// <summary>
    /// チュートリアルステータス
    /// </summary>
    public class TutorialStatus : MonoBehaviour
    {
        /// <summary>
        /// エディタ把握用パラメーター
        /// </summary>
        [SerializeField] private GameState currentGameState;

        [SerializeField] private PlayerMoveCore _playerMoveCore;
        [SerializeField] private PlayerLaserTurret _playerLaserTurret;
        [SerializeField] private SubWeaponTurret _subWeaponTurret;
        [SerializeField] private BombTurret _bombTurret;
        [SerializeField] private EnemyShield _tutorialLaserTarget;
        [SerializeField] private TutorialCapsule _tutorialCapsule;

        private Vector3 _initialPosition;

        public readonly GameStateReactiveProperty CurrentGameStateReactiveProperty = new GameStateReactiveProperty();
        private void Start()
        {
            _initialPosition = _playerMoveCore.transform.position;

            _playerLaserTurret.gameObject.SetActive(false);
            _subWeaponTurret.gameObject.SetActive(false);
            _bombTurret.gameObject.SetActive(false);
            _tutorialLaserTarget.gameObject.SetActive(false);
            _tutorialCapsule.gameObject.SetActive(false);

            MessageBroker.Default.Publish(new TutorialStart(TalkEnum.TalkStart));

            // 移動チュートリアル
            MessageBroker.Default.Receive<TutorialMove>()
                .Subscribe(_ => TutorialMoveCheck())
                .AddTo(this);

            // TutorialLaser受信後にレーザーを有効
            MessageBroker.Default.Receive<TutorialLaser>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ =>
                {
                    _playerLaserTurret.LaserEnable();
                    _tutorialLaserTarget.gameObject.SetActive(true);
                })
                .AddTo(this);

            // _tutorialCapsule
            MessageBroker.Default.Receive<TutorialItem>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ =>
                {
                    _tutorialCapsule.gameObject.SetActive(true);
                })
                .AddTo(this);

            // TutorialSubWeapon受信後にサブウェポンを有効
            MessageBroker.Default.Receive<TutorialSubWeapon>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ =>
                {
                    _subWeaponTurret.SubWeaponEnable();
                    TutorialSubWeaponCheck();
                })
                .AddTo(this);

            // TutorialBomb受信後にボムを有効
            MessageBroker.Default.Receive<TutorialBomb>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ =>
                {
                    _bombTurret.BombEnable();
                    TutorialBombCheck();
                })
                .AddTo(this);
        }

        private void TutorialMoveCheck()
        {
            Observable.EveryUpdate()
                .Where(_ => Vector3.Distance(_initialPosition, _playerMoveCore.transform.position) > 10)
                .Take(1)
                .Subscribe(_ =>
                {
                    MessageBroker.Default.Publish(new TutorialLaser(TalkEnum.TalkStart));
                })
                .AddTo(this);
        }

        private void TutorialSubWeaponCheck()
        {
            // サブウェポンを一度でも使ったら購読終了、次のチュートリアルへ
            MessageBroker.Default
                .Receive<SubWeaponFired>()
                .Take(1)
                .Subscribe(_ =>
                {
                    MessageBroker.Default.Publish(new TutorialBomb(TalkEnum.TalkStart));
                })
                .AddTo(this);
        }

        private void TutorialBombCheck()
        {
            // ボム使用チェック、使用したら購読終了、チュートリアル終了
            MessageBroker.Default
                .Receive<BombUsed>()
                .Take(1)
                .Subscribe(_ =>
                {
                    MessageBroker.Default.Publish(new TutorialEnd(TalkEnum.TalkStart));
                })
                .AddTo(this);
        }
    }
}
