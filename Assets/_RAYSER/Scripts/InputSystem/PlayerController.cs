using _RAYSER.Scripts.Bomb;
using _RAYSER.Scripts.Item;
using _RAYSER.Scripts.SubWeapon;
using Event;
using Event.Signal;
using MessagePipe;
using PlayerMove;
using Turret;
using UI.Game;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    /// <summary>
    /// InputSystem自機コントローラー用クラス
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        [SerializeField] private PlayerMoveCore _playerMove;
        [SerializeField] private PlayerLaserTurret _turret;

        private IPublisher<SubweaponMoveDirection> _subweaponMoveDirectionPublisher;
        private IPublisher<SubweaponUseSignal> _subweaponUseSignalPublisher;
        private IPublisher<BombUseSignal> _bombUseSignalPublisher;
        private ItemAcquisition _itemAcquisition;

        public void Setup(
            IPublisher<SubweaponMoveDirection> subweaponMoveDirectionPublisher,
            IPublisher<SubweaponUseSignal> subweaponUseSignalPublisher,
            IPublisher<BombUseSignal> bombUseSignalPublisher,
            ItemAcquisition itemAcquisition
            )
        {
            _subweaponMoveDirectionPublisher = subweaponMoveDirectionPublisher;
            _subweaponUseSignalPublisher = subweaponUseSignalPublisher;
            _bombUseSignalPublisher = bombUseSignalPublisher;
            _itemAcquisition = itemAcquisition;
        }

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();

            MessageBroker.Default.Receive<GameStartEventEnd>()
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Enable();
                    _playerInputActions.UI.Disable();
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage2Start>()
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Enable();
                    _playerInputActions.UI.Disable();
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage3Start>()
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Enable();
                    _playerInputActions.UI.Disable();
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage2IntervalStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Disable();
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Stage3IntervalStart>()
                .Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Disable();
                })
                .AddTo(this);

            MessageBroker.Default.Receive<Gameover>()
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Disable();
                    _playerInputActions.UI.Enable();
                })
                .AddTo(this);

            MessageBroker.Default.Receive<GameClear>()
                .Where(x=>x._talk == TalkEnum.TalkStart)
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Disable();
                })
                .AddTo(this);

            MessageBroker.Default.Receive<GameClear>()
                .Where(x=>x._talk == TalkEnum.TalkEnd)
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Disable();
                    _playerInputActions.UI.Enable();
                })
                .AddTo(this);

        }

        private void OnEnable()
        {
            _playerInputActions.Player.Fire.performed += OnFire;
            _playerInputActions.Player.Fire.canceled += OnFireStop;
            _playerInputActions.Player.Move.performed += OnMove;
            _playerInputActions.Player.Move.canceled += OnMoveStop;
            _playerInputActions.Player.SubWeapon.performed += OnSubWeaponUse;
            _playerInputActions.Player.SubWeapon.canceled += OnSubWeaponStop;
            _playerInputActions.Player.Bomb.performed += OnBombUse;
            _playerInputActions.Player.Bomb.canceled += OnBombStop;
            _playerInputActions.Player.WeaponSwitchRight.performed += OnWeaponSwitchRight;
            _playerInputActions.Player.WeaponSwitchLeft.performed += OnWeaponSwitchLeft;
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Fire.performed -= OnFire;
            _playerInputActions.Player.Fire.canceled -= OnFireStop;
            _playerInputActions.Player.Move.performed -= OnMove;
            _playerInputActions.Player.Move.canceled -= OnMoveStop;
            _playerInputActions.Player.SubWeapon.performed -= OnSubWeaponUse;
            _playerInputActions.Player.SubWeapon.canceled -= OnSubWeaponStop;
            _playerInputActions.Player.Bomb.performed -= OnBombUse;
            _playerInputActions.Player.WeaponSwitchRight.performed -= OnWeaponSwitchRight;
            _playerInputActions.Player.WeaponSwitchLeft.performed -= OnWeaponSwitchLeft;
        }

        private void OnFire(InputAction.CallbackContext obj)
        {
            _turret.Fire();
        }

        private void OnFireStop(InputAction.CallbackContext obj)
        {
            _turret.FireStop();
        }

        private void OnMoveStop(InputAction.CallbackContext obj)
        {
            _playerMove.SetDirection(Vector2.zero);
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            var moveValue = obj.ReadValue<Vector2>();
            _playerMove.SetDirection(moveValue);
        }

        /// <summary>
        /// サブウェポンの使用
        /// </summary>
        /// <param name="context"></param>
        private void OnSubWeaponUse(InputAction.CallbackContext context)
        {
            var selectedSubWeapon = _itemAcquisition.GetSelectingSubWeapon();
            if (selectedSubWeapon == null)
            {
                Debug.LogError("Selected subweapon is null");
                return;
            }

            _subweaponUseSignalPublisher.Publish(new SubweaponUseSignal(SubweaponUseType.Use));
        }

        /// <summary>
        /// サブウェポンの使用停止
        /// </summary>
        /// <param name="context"></param>
        private void OnSubWeaponStop(InputAction.CallbackContext context)
        {
            _subweaponUseSignalPublisher.Publish(new SubweaponUseSignal(SubweaponUseType.Stop));
        }

        /**
         * ボムの使用
         */
        private void OnBombUse(InputAction.CallbackContext context)
        {
            Debug.Log("OnBombUse");
            _bombUseSignalPublisher.Publish(new BombUseSignal(BombUseType.Use));
        }

        /**
         * ボムの使用停止
         */
        private void OnBombStop(InputAction.CallbackContext context)
        {
            _bombUseSignalPublisher.Publish(new BombUseSignal(BombUseType.Stop));
        }

        /**
         * 次のサブウェポンの切り替え
         */
        private void OnWeaponSwitchRight(InputAction.CallbackContext context)
        {
            _subweaponMoveDirectionPublisher.Publish(SubweaponMoveDirection.Right);
        }

        /**
         * 前のサブウェポンの切り替え
         */
        private void OnWeaponSwitchLeft(InputAction.CallbackContext context)
        {
            _subweaponMoveDirectionPublisher.Publish(SubweaponMoveDirection.Left);
        }
    }
}
