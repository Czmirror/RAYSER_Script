using Event;
using Event.Signal;
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

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();

            MessageBroker.Default.Receive<Gameover>()
                .Subscribe(x =>
                {
                    _playerInputActions.Player.Disable();
                    _playerInputActions.UI.Enable();
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
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Fire.performed -= OnFire;
            _playerInputActions.Player.Fire.canceled -= OnFireStop;
            _playerInputActions.Player.Move.performed -= OnMove;
            _playerInputActions.Player.Move.canceled -= OnMoveStop;
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
    }
}
